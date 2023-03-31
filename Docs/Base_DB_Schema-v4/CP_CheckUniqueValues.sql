CREATE PROCEDURE CheckUniqueValueForInsert
    @tablename varchar(50),
	@fieldname varchar(50),
	@fieldvalue varchar(100)
AS
BEGIN
    
	DECLARE @SQLString NVARCHAR(500);  
	DECLARE @ParmDefinition NVARCHAR(500);  

	set @SQLString = 
		N'select count(*) cnt from ' 
		+ @tablename + 
		' where ' + @fieldname + '=@value';
	
	set @ParmDefinition = N'@value varchar(255)';

	print @SQLString;

	EXECUTE sp_executesql @SQLString, @ParmDefinition,
		 @value=@fieldvalue;  
END

-- 


CREATE PROCEDURE CheckUniqueValueForUpdate
    @tablename varchar(50),
	@fieldname varchar(50),
	@fieldvalue varchar(100),
	@recordfieldname varchar(100),
	@recordID bigint
AS
BEGIN
    
	DECLARE @SQLString NVARCHAR(500);  
	DECLARE @ParmDefinition NVARCHAR(500);  

	set @SQLString = 
		N'select count(*) cnt from ' 
		+ @tablename + 
		' where ' + @fieldname + '=@value' 
		+ ' and ' + @recordfieldname + '<>@id' ;
	
	set @ParmDefinition = N'@value varchar(255), @id bigint';

	print @SQLString;

	EXECUTE sp_executesql @SQLString, @ParmDefinition,
		 @value=@fieldvalue, @id=@recordID;  
END