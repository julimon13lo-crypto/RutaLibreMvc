CREATE DATABASE RutaLibreDB;

USE RutaLibreDB;

CREATE TABLE Categorias (
    CategoriaID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NULL
);

CREATE TABLE Vehiculos (
    VehiculoID INT IDENTITY(1,1) PRIMARY KEY,
    Placa VARCHAR(10) NOT NULL UNIQUE,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    [Año] INT NOT NULL,
    PrecioDia DECIMAL(10, 2) NOT NULL,
    Transmision VARCHAR(20) NOT NULL DEFAULT 'Manual',
    Estado VARCHAR(20) NOT NULL DEFAULT 'Disponible',
    CategoriaID INT NOT NULL,
    CONSTRAINT FK_Vehiculos_Categorias FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID)
);

CREATE TABLE Clientes (
    ClienteID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Cedula VARCHAR(20) NOT NULL UNIQUE,
    Telefono VARCHAR(20) NULL,
    Email VARCHAR(100) NULL
);


CREATE TABLE Rentas (
    RentaID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    ClienteID INT NOT NULL,
    FechaRecogida DATETIME NOT NULL,
    FechaDevolucion DATETIME NOT NULL,
    TotalEstimado DECIMAL(10, 2) NOT NULL,
    EstadoRenta VARCHAR(20) NOT NULL DEFAULT 'Activa',
    FechaRegistro DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Rentas_Vehiculos FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    CONSTRAINT FK_Rentas_Clientes FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);


CREATE TABLE Pagos (
    PagoID INT IDENTITY(1,1) PRIMARY KEY,
    RentaID INT NOT NULL,
    FechaPago DATETIME DEFAULT GETDATE(),
    Monto DECIMAL(10, 2) NOT NULL,
    MetodoPago VARCHAR(30) NOT NULL, 
    CONSTRAINT FK_Pagos_Rentas FOREIGN KEY (RentaID) REFERENCES Rentas(RentaID)
);


CREATE TABLE Empleados (
    EmpleadoID INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Cedula VARCHAR(20) NOT NULL UNIQUE,
    Cargo VARCHAR(50) NOT NULL DEFAULT 'Asesor de Ventas',
    Telefono VARCHAR(20) NULL,
    Estado VARCHAR(20) NOT NULL DEFAULT 'Activo'
);


CREATE TABLE Mantenimientos (
    MantenimientoID INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID INT NOT NULL,
    FechaMantenimiento DATETIME NOT NULL DEFAULT GETDATE(),
    TipoMantenimiento VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Costo DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_Mantenimientos_Vehiculos FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID)
);


ALTER TABLE Rentas 
ADD EmpleadoID INT NULL;

ALTER TABLE Rentas 
ADD CONSTRAINT FK_Rentas_Empleados FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID);