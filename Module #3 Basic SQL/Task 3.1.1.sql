IF NOT EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[dbo].[Crds]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Crds] (
		[Car] [int] IDENTITY(1,1) NOT NULL,
		CONSTRAINT [PK_Cat] PRIMARY KEY ([Car] ASC)	
	)
END