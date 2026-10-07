using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RutaLibreMvc.Data;
using RutaLibreMvc.Models;
using RutaLibreMvc.Models.ViewModel;

namespace RutaLibreMvc.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _context;

        public VehiculosController(AppDbContext context)
        {
            _context = context;
        }

        // Carga desde la base de datos las listas que usa la vista
        private async Task CargarListasAsync(RentaViewModel model)
        {
            model.Vehiculos = await _context.Vehiculos
                .Include(v => v.Categoria)
                .OrderBy(v => v.Id)
                .ToListAsync();

            model.Categorias = await _context.Categorias
                .OrderBy(c => c.NombreCategoria)
                .ToListAsync();

            model.Empleados = await _context.Empleados
                .Where(e => e.Estado == "Activo")
                .OrderBy(e => e.NombreCompleto)
                .ToListAsync();
        }

        private static int CalcularDias(DateTime recogida, DateTime devolucion)
        {
            int dias = (devolucion.Date - recogida.Date).Days;
            return dias <= 0 ? 1 : dias;
        }

        public async Task<IActionResult> Index()
        {
            var model = new RentaViewModel
            {
                FechaRecogida = DateTime.Today,
                FechaDevolucion = DateTime.Today.AddDays(1)
            };
            await CargarListasAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CrearVehiculo(Vehiculo nuevoVehiculo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Mensaje"] = "Error al registrar el vehículo. Verifique los datos ingresados.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Index");
            }

            nuevoVehiculo.Placa = nuevoVehiculo.Placa.ToUpper().Trim();
            nuevoVehiculo.Estado = "Disponible";

            if (await _context.Vehiculos.AnyAsync(v => v.Placa == nuevoVehiculo.Placa))
            {
                TempData["Mensaje"] = $"Ya existe un vehículo con la placa {nuevoVehiculo.Placa}.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Index");
            }

            if (!await _context.Categorias.AnyAsync(c => c.CategoriaID == nuevoVehiculo.CategoriaId))
            {
                TempData["Mensaje"] = "La categoría seleccionada no existe.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Index");
            }

            _context.Vehiculos.Add(nuevoVehiculo);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"Vehículo ({nuevoVehiculo.TipoVehiculo}) con placa {nuevoVehiculo.Placa} registrado exitosamente.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditarVehiculo(Vehiculo vehiculoEdit)
        {
            var vehiculoExistente = await _context.Vehiculos.FindAsync(vehiculoEdit.Id);
            if (vehiculoExistente == null)
            {
                TempData["Mensaje"] = "No se encontró el vehículo para actualizar.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Index");
            }

            var placa = (vehiculoEdit.Placa ?? string.Empty).ToUpper().Trim();
            if (await _context.Vehiculos.AnyAsync(v => v.Placa == placa && v.Id != vehiculoEdit.Id))
            {
                TempData["Mensaje"] = $"Ya existe otro vehículo con la placa {placa}.";
                TempData["TipoMensaje"] = "danger";
                return RedirectToAction("Index");
            }

            vehiculoExistente.Placa = placa;
            vehiculoExistente.TipoVehiculo = vehiculoEdit.TipoVehiculo;
            vehiculoExistente.Marca = vehiculoEdit.Marca;
            vehiculoExistente.Modelo = vehiculoEdit.Modelo;
            vehiculoExistente.Anio = vehiculoEdit.Anio;
            vehiculoExistente.PrecioDia = vehiculoEdit.PrecioDia;
            vehiculoExistente.CategoriaId = vehiculoEdit.CategoriaId;
            vehiculoExistente.Transmision = vehiculoEdit.Transmision;
            vehiculoExistente.Estado = vehiculoEdit.Estado;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"Vehículo {vehiculoExistente.Placa} actualizado con éxito.";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CalcularCosto(RentaViewModel model)
        {
            await CargarListasAsync(model);

            if (model.VehiculoSeleccionadoId.HasValue)
            {
                var vehiculo = model.Vehiculos.FirstOrDefault(v => v.Id == model.VehiculoSeleccionadoId.Value);
                if (vehiculo != null)
                {
                    int dias = CalcularDias(model.FechaRecogida, model.FechaDevolucion);
                    model.TotalEstimado = dias * vehiculo.PrecioDia;
                    model.Mensaje = $"Costo para {dias} día(s) con [{vehiculo.TipoVehiculo}] {vehiculo.Marca} {vehiculo.Modelo}. Total: ${model.TotalEstimado:N2}";
                    model.TipoMensaje = "info";
                }
            }
            else
            {
                model.Mensaje = "Por favor seleccione un vehículo de la tabla.";
                model.TipoMensaje = "warning";
            }

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarRenta(RentaViewModel model)
        {
            await CargarListasAsync(model);

            if (!model.VehiculoSeleccionadoId.HasValue)
            {
                model.Mensaje = "Seleccione un vehículo disponible para rentar.";
                model.TipoMensaje = "danger";
                return View("Index", model);
            }

            if (string.IsNullOrWhiteSpace(model.NombreCompleto) || string.IsNullOrWhiteSpace(model.Cedula))
            {
                model.Mensaje = "Ingrese el nombre completo y el documento de identidad del cliente.";
                model.TipoMensaje = "danger";
                return View("Index", model);
            }

            var vehiculo = await _context.Vehiculos.FindAsync(model.VehiculoSeleccionadoId.Value);
            if (vehiculo == null || vehiculo.Estado != "Disponible")
            {
                model.Mensaje = "El vehículo seleccionado no está disponible para renta.";
                model.TipoMensaje = "danger";
                return View("Index", model);
            }

            // La cédula es UNIQUE en la tabla Clientes: si ya existe, se reutiliza el cliente
            var cedula = model.Cedula.Trim();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula);
            if (cliente == null)
            {
                cliente = new Cliente { Cedula = cedula };
                _context.Clientes.Add(cliente);
            }
            cliente.NombreCompleto = model.NombreCompleto.Trim();
            if (!string.IsNullOrWhiteSpace(model.Telefono)) cliente.Telefono = model.Telefono.Trim();
            if (!string.IsNullOrWhiteSpace(model.Email)) cliente.Email = model.Email.Trim();

            int dias = CalcularDias(model.FechaRecogida, model.FechaDevolucion);
            decimal total = dias * vehiculo.PrecioDia;

            var renta = new Renta
            {
                Vehiculo = vehiculo,
                Cliente = cliente,
                EmpleadoId = model.EmpleadoId,
                FechaRecogida = model.FechaRecogida,
                FechaDevolucion = model.FechaDevolucion,
                TotalEstimado = total,
                EstadoRenta = "Activa",
                FechaRegistro = DateTime.Now
            };
            renta.Pagos.Add(new Pago
            {
                Monto = total,
                MetodoPago = string.IsNullOrWhiteSpace(model.MetodoPago) ? "Efectivo" : model.MetodoPago,
                FechaPago = DateTime.Now
            });
            _context.Rentas.Add(renta);

            vehiculo.Estado = "Rentado";

            // Un solo SaveChanges: cliente, renta, pago y estado del vehículo se guardan juntos
            await _context.SaveChangesAsync();

            await CargarListasAsync(model);
            model.Mensaje = $"¡Renta #{renta.Id} registrada con éxito para {cliente.NombreCompleto}! [{vehiculo.TipoVehiculo}] Placa: {vehiculo.Placa}. Total: ${total:N2} ({renta.Pagos[0].MetodoPago})";
            model.TipoMensaje = "success";
            model.TotalEstimado = total;
            model.VehiculoSeleccionadoId = null;

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarDevolucion(RentaViewModel model)
        {
            await CargarListasAsync(model);

            if (!model.VehiculoSeleccionadoId.HasValue)
            {
                model.Mensaje = "Seleccione un vehículo para devolver.";
                model.TipoMensaje = "danger";
                return View("Index", model);
            }

            var vehiculo = await _context.Vehiculos.FindAsync(model.VehiculoSeleccionadoId.Value);
            if (vehiculo == null || vehiculo.Estado != "Rentado")
            {
                model.Mensaje = "El vehículo seleccionado no figura como 'Rentado'.";
                model.TipoMensaje = "warning";
                return View("Index", model);
            }

            // Cierra las rentas activas de ese vehículo
            var rentasActivas = await _context.Rentas
                .Where(r => r.VehiculoId == vehiculo.Id && r.EstadoRenta == "Activa")
                .ToListAsync();
            foreach (var r in rentasActivas)
                r.EstadoRenta = "Finalizada";

            vehiculo.Estado = "Disponible";
            await _context.SaveChangesAsync();

            await CargarListasAsync(model);
            model.Mensaje = $"Devolución procesada correctamente. [{vehiculo.TipoVehiculo}] Placa {vehiculo.Placa} ({vehiculo.Marca} {vehiculo.Modelo}) ahora está 'Disponible'.";
            model.TipoMensaje = "success";
            model.VehiculoSeleccionadoId = null;

            return View("Index", model);
        }
    }
}
