  set nocount on;
  DECLARE @Latitude NVARCHAR(555) 
  DECLARE @Longitude NVARCHAR(555)
  DECLARE @Id int
  DECLARE @PostCode nvarchar(555) 
  DECLARE @CursorContent CURSOR 
  SET @CursorContent = CURSOR FOR SELECT Latitude,Longitude,Id,PostCode  FROM Content where NearByCompanyIds is null
  OPEN @CursorContent 
  FETCH NEXT FROM @CursorContent INTO  @Latitude, @Longitude, @Id, @PostCode

  WHILE ( @@FETCH_STATUS = 0) 
  BEGIN 
		  Update Content set NearByCompanyIds  = (SELECT STUFF(( select top 5 ',' + Convert(nvarchar,Id) from Content as c 
		      CROSS APPLY (SELECT cos(radians(@Latitude)) * cos(radians(c.Latitude)) * cos(radians(c.Longitude) - radians(@Longitude)) + 
			  sin(radians(@Latitude)) * sin(radians(c.Latitude))) T(ACosInput) CROSS APPLY (SELECT ((3959 * acos(CASE WHEN ABS(ACosInput) > 1 THEN SIGN(ACosInput)*1 ELSE ACosInput END)))) T2(distance)  
			  where c.PostCode = @PostCode and  c.Id <> @Id order by distance  FOR XML PATH('')) ,1,1,'')
			  ) 
			  where Id = @Id
              
  FETCH NEXT FROM @CursorContent INTO @Latitude, @Longitude, @Id, @PostCode
  END 
  CLOSE @CursorContent 
  DEALLOCATE @CursorContent 
