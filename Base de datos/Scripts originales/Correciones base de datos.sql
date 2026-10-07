USE RutaLibreDB;


DELETE FROM Vehiculos WHERE VehiculoID = 7;


DBCC CHECKIDENT ('Vehiculos', RESEED, 3);

EXEC sp_rename 'Vehiculos.Anio', 'Año', 'COLUMN';


DELETE FROM Pagos 
WHERE RentaID IN (1, 4);


DELETE FROM Rentas 
WHERE RentaID IN (1, 4);