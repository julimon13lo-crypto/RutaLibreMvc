/* =====================================================================
   RutaLibreDB - INSTALACIÓN COMPLETA (base de datos + datos de ejemplo)
   ---------------------------------------------------------------------
   Úsalo SOLO en un PC donde la base RutaLibreDB NO exista todavía
   (por ejemplo, el computador del profesor o uno nuevo).
   Deja la base igual a la tuya ya corregida e integrada con el programa.
   Si ya tienes RutaLibreDB creada, usa "Integracion con el programa.sql".
   ===================================================================== */
IF DB_ID(N'RutaLibreDB') IS NULL
    CREATE DATABASE RutaLibreDB;
GO

USE RutaLibreDB;
GO

/* ----------------------------- TABLAS ----------------------------- */
CREATE TABLE Categorias (
    CategoriaID     INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(50)  NOT NULL UNIQUE,
    Descripcion     VARCHAR(200) NULL
);

CREATE TABLE Vehiculos (
    VehiculoID   INT IDENTITY(1,1) PRIMARY KEY,
    Placa        VARCHAR(10)    NOT NULL UNIQUE,
    TipoVehiculo VARCHAR(10)    NOT NULL CONSTRAINT DF_Vehiculos_TipoVehiculo DEFAULT 'Carro',
    Marca        VARCHAR(50)    NOT NULL,
    Modelo       VARCHAR(50)    NOT NULL,
    [Año]        INT            NOT NULL,
    PrecioDia    DECIMAL(10, 2) NOT NULL,
    Transmision  VARCHAR(20)    NOT NULL DEFAULT 'Manual',
    Estado       VARCHAR(20)    NOT NULL DEFAULT 'Disponible',
    CategoriaID  INT            NOT NULL,
    CONSTRAINT FK_Vehiculos_Categorias FOREIGN KEY (CategoriaID) REFERENCES Categorias(CategoriaID),
    CONSTRAINT CK_Vehiculos_TipoVehiculo CHECK (TipoVehiculo IN ('Carro', 'Moto'))
);

CREATE TABLE Clientes (
    ClienteID      INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Cedula         VARCHAR(20)  NOT NULL UNIQUE,
    Telefono       VARCHAR(20)  NULL,
    Email          VARCHAR(100) NULL
);

CREATE TABLE Empleados (
    EmpleadoID     INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Cedula         VARCHAR(20)  NOT NULL UNIQUE,
    Cargo          VARCHAR(50)  NOT NULL DEFAULT 'Asesor de Ventas',
    Telefono       VARCHAR(20)  NULL,
    Estado         VARCHAR(20)  NOT NULL DEFAULT 'Activo'
);

CREATE TABLE Rentas (
    RentaID         INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID      INT            NOT NULL,
    ClienteID       INT            NOT NULL,
    FechaRecogida   DATETIME       NOT NULL,
    FechaDevolucion DATETIME       NOT NULL,
    TotalEstimado   DECIMAL(10, 2) NOT NULL,
    EstadoRenta     VARCHAR(20)    NOT NULL DEFAULT 'Activa',
    FechaRegistro   DATETIME       DEFAULT GETDATE(),
    EmpleadoID      INT            NULL,
    CONSTRAINT FK_Rentas_Vehiculos FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID),
    CONSTRAINT FK_Rentas_Clientes  FOREIGN KEY (ClienteID)  REFERENCES Clientes(ClienteID),
    CONSTRAINT FK_Rentas_Empleados FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
);

CREATE TABLE Pagos (
    PagoID     INT IDENTITY(1,1) PRIMARY KEY,
    RentaID    INT            NOT NULL,
    FechaPago  DATETIME       DEFAULT GETDATE(),
    Monto      DECIMAL(10, 2) NOT NULL,
    MetodoPago VARCHAR(30)    NOT NULL,
    CONSTRAINT FK_Pagos_Rentas FOREIGN KEY (RentaID) REFERENCES Rentas(RentaID)
);

CREATE TABLE Mantenimientos (
    MantenimientoID    INT IDENTITY(1,1) PRIMARY KEY,
    VehiculoID         INT            NOT NULL,
    FechaMantenimiento DATETIME       NOT NULL DEFAULT GETDATE(),
    TipoMantenimiento  VARCHAR(50)    NOT NULL,
    Descripcion        VARCHAR(250)   NULL,
    Costo              DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_Mantenimientos_Vehiculos FOREIGN KEY (VehiculoID) REFERENCES Vehiculos(VehiculoID)
);
GO

/* ------------------------- DATOS DE EJEMPLO ------------------------ */
INSERT INTO Categorias (NombreCategoria, Descripcion) VALUES
('Económico', 'Vehículos compactos de bajo consumo'),
('SUV',       'Camionetas familiares'),
('Moto',      'Motocicletas urbanas y deportivas');

INSERT INTO Vehiculos (Placa, TipoVehiculo, Marca, Modelo, [Año], PrecioDia, Transmision, Estado, CategoriaID) VALUES
('KFX-842', 'Carro', 'Toyota',   'Corolla',  2021,  30.00, 'Automática', 'Disponible', 1),
('UWL-193', 'Carro', 'Honda',    'CR-V',     2022,  45.00, 'Automática', 'Rentado',    2),
('MWX-81D', 'Moto',  'Yamaha',   'NMAX 155', 2023,  22.00, 'Automática', 'Disponible', 3),
('JDA-56T', 'Moto',  'Kawasaki', 'Z1000',    2027, 100.00, 'Manual',     'Disponible', 3);

INSERT INTO Clientes (NombreCompleto, Cedula, Telefono, Email) VALUES
('Juan Pérez',  '1012345678', '3001234567', 'juan.perez@email.com'),
('María Gómez', '1098765432', '3109876543', 'maria.gomez@email.com');

INSERT INTO Empleados (NombreCompleto, Cedula, Cargo, Telefono) VALUES
('Carlos Andrés Mendoza', '1018452963', 'Asesor de Ventas',    '3104567890'),
('Laura Sofía Gómez',     '1020304050', 'Gerente de Sucursal', '3159876543'),
('Marta Lucía Ramírez',   '1032165498', 'Asesor de Ventas',    '3201234567');

INSERT INTO Rentas (VehiculoID, ClienteID, FechaRecogida, FechaDevolucion, TotalEstimado, EstadoRenta, EmpleadoID) VALUES
(2, 1, '2026-09-20', '2026-09-23', 135.00, 'Activa', 2);

INSERT INTO Pagos (RentaID, Monto, MetodoPago) VALUES
(1, 135.00, 'Tarjeta');

INSERT INTO Mantenimientos (VehiculoID, TipoMantenimiento, Descripcion, Costo) VALUES
(1, 'Cambio de Aceite',   'Mantenimiento preventivo de los 10,000 km', 150000.00),
(2, 'Revisión de Frenos', 'Cambio de pastillas de freno delanteras',   280000.00),
(3, 'Sincronización',     'Mantenimiento general de motor y bujías',   120000.00);
GO

SELECT * FROM Vehiculos;
GO
