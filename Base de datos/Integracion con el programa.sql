/* =====================================================================
   RutaLibreDB - Script de INTEGRACIÓN con el programa RutaLibreMvc
   ---------------------------------------------------------------------
   Ejecutar UNA vez sobre la base RutaLibreDB que ya tienes creada
   (después de "Tablas sql.sql", "Insertar datos.sql" y
   "Correciones base de datos.sql").
   Se puede volver a ejecutar sin problema: solo hace cambios si faltan.
   ===================================================================== */
USE RutaLibreDB;
GO

/* 1. La columna del año debe llamarse [Año]
      (por si en algún momento quedó como "Anio") */
IF COL_LENGTH(N'dbo.Vehiculos', N'Año') IS NULL
   AND COL_LENGTH(N'dbo.Vehiculos', N'Anio') IS NOT NULL
BEGIN
    EXEC sp_rename N'dbo.Vehiculos.Anio', N'Año', N'COLUMN';
    PRINT N'Columna Anio renombrada a Año.';
END
GO

/* 2. El programa distingue entre Carro y Moto (filtro, íconos, formularios).
      Se agrega la columna TipoVehiculo; por defecto 'Carro' y los
      vehículos de la categoría 'Moto' quedan como 'Moto'. */
IF COL_LENGTH(N'dbo.Vehiculos', N'TipoVehiculo') IS NULL
BEGIN
    ALTER TABLE dbo.Vehiculos
        ADD TipoVehiculo VARCHAR(10) NOT NULL
            CONSTRAINT DF_Vehiculos_TipoVehiculo DEFAULT 'Carro';

    EXEC (N'UPDATE v
               SET v.TipoVehiculo = ''Moto''
              FROM dbo.Vehiculos v
              INNER JOIN dbo.Categorias c ON c.CategoriaID = v.CategoriaID
             WHERE c.NombreCategoria = ''Moto'';');

    PRINT N'Columna TipoVehiculo agregada.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Vehiculos_TipoVehiculo')
    ALTER TABLE dbo.Vehiculos
        ADD CONSTRAINT CK_Vehiculos_TipoVehiculo CHECK (TipoVehiculo IN ('Carro', 'Moto'));
GO

/* 3. Verificación */
SELECT VehiculoID, TipoVehiculo, Placa, Marca, Modelo, [Año], PrecioDia, Transmision, Estado, CategoriaID
FROM dbo.Vehiculos;
GO
