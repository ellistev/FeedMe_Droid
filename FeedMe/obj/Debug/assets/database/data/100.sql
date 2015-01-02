 INSERT INTO Locations (Id,Name)
VALUES 
(1, 'Bedroom'),
(2 'Couch'),
(3, 'Table');
GO

INSERT INTO MMLocation (Id, Major, Minor, LocationId)
VALUES
(1, 1854, 36039, 1),
(2, 43942, 1810, 2),
(3, 62614, 62288, 3);
GO

INSERT INTO DBDataVersion (LastDBDataVersion)
VALUES
(100);
GO

INSERT INTO DBSchemaVersion (LastSchemaVersion)
VALUES
(100);
GO