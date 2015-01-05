 INSERT INTO Locations (Id,Name)
VALUES 
(1, 'Not Found'),
(2, 'Bedroom'),
(3, 'Couch'),
(4, 'Table');
GO

INSERT INTO MMLocation (Id, UUID, Major, Minor, LocationId)
VALUES
(1, '', 0, 0, 1),
(2, '', 1854, 36039, 2),
(3, '', 43942, 1810, 3),
(4, '', 62614, 62288, 4);
GO

INSERT INTO DBDataVersion (LastDBDataVersion)
VALUES
(100);
GO

INSERT INTO DBSchemaVersion (LastDBSchemaVersion)
VALUES
(100);
GO