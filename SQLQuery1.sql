 SELECT * FROM Request
 JOIN Header ON Request.R_ID = Header.R_ID
 WHERE R_DATE=(SELECT max(R_DATE) FROM Request);


-- Extract Request's ID from Request Table
DECLARE @ID INTEGER;
SELECT @ID = R_ID FROM Request
WHERE R_DATE=(SELECT max(R_DATE) FROM Request);
         
-- Get the Row            
SELECT * FROM Request
WHERE R_DATE=(SELECT max(R_DATE) FROM Request);
                
-- Get Headers
SELECT * FROM Header
WHERE R_ID = @ID;