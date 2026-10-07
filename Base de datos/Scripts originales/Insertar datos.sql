USE RutaLibreDB;
GO


INSERT INTO Categorias (NombreCategoria, Descripcion) VALUES 
('Económico', 'Vehículos compactos de bajo consumo'),
('SUV', 'Camionetas familiares'),
('Moto', 'Motocicletas urbanas y deportivas');


INSERT INTO Vehiculos (Placa, Marca, Modelo, [Año], PrecioDia, Transmision, Estado, CategoriaID) VALUES 
('KFX-842', 'Toyota', 'Corolla', 2021, 30.00, 'Automática', 'Disponible', 1),
('UWL-193', 'Honda', 'CR-V', 2022, 45.00, 'Automática', 'Rentado', 2),
('MWX-81D', 'Yamaha', 'NMAX 155', 2023, 22.00, 'Automática', 'Disponible', 3);
INSERT INTO Vehiculos (Placa, Marca, Modelo, [Año], PrecioDia, Transmision, Estado, CategoriaID) VALUES 
('JDA-56T', 'Kawasaki', 'Z1000', 2027, 100.00, 'Manual', 'Disponible', 3);


INSERT INTO Clientes (NombreCompleto, Cedula, Telefono, Email) VALUES 
('Juan Pérez', '1012345678', '3001234567', 'juan.perez@email.com'),
('María Gómez', '1098765432', '3109876543', 'maria.gomez@email.com');


INSERT INTO Rentas (VehiculoID, ClienteID, FechaRecogida, FechaDevolucion, TotalEstimado, EstadoRenta, EmpleadoID) VALUES 
(2, 1, '2026-09-20', '2026-09-23', 135.00, 'Activa', 2);


INSERT INTO Pagos (RentaID, Monto, MetodoPago) VALUES 
(1, 135.00, 'Tarjeta');

INSERT INTO Empleados (NombreCompleto, Cedula, Cargo, Telefono) VALUES 
('Carlos Andrés Mendoza', '1018452963', 'Asesor de Ventas', '3104567890'),
('Laura Sofía Gómez', '1020304050', 'Gerente de Sucursal', '3159876543'),
('Marta Lucía Ramírez', '1032165498', 'Asesor de Ventas', '3201234567');

INSERT INTO Mantenimientos (VehiculoID, TipoMantenimiento, Descripcion, Costo) VALUES 
(1, 'Cambio de Aceite', 'Mantenimiento preventivo de los 10,000 km', 150000.00),
(2, 'Revisión de Frenos', 'Cambio de pastillas de freno delanteras', 280000.00),
(3, 'Sincronización', 'Mantenimiento general de motor y bujías', 120000.00);

INSERT INTO Rentas (VehiculoID, ClienteID, FechaRecogida, FechaDevolucion, TotalEstimado, EstadoRenta, EmpleadoID) VALUES 
(2, 1, '2026-09-25', '2026-09-30', 135.00, 'Activa', 1);