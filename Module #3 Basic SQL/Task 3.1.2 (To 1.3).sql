IF NOT EXISTS (SELECT * FROM SYS.OBJECTS
WHERE OBJECT_ID = OBJECT_ID(N'[dbo].[Regions]') AND TYPE IN (N'U'))
BEGIN
	EXEC SP_RENAME 'dbo.Region', 'Regions';
	ALTER TABLE Customers ADD DateCreation DATE;
END