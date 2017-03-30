﻿CREATE TABLE Request (
	R_ID INT PRIMARY KEY IDENTITY(100,1),
	R_URL VARCHAR(255) NOT NULL,
	R_METHOD VARCHAR(16) NOT NULL,
	R_DATE DATE NOT NULL,
	R_CTYPE VARCHAR(32) NOT NULL,
	R_CONTROLLER VARCHAR(255) NOT NULL,
	R_PARAMETERS VARCHAR(255) NOT NULL,
	R_BODY TEXT
);


CREATE TABLE Header (
	R_ID INT NOT NULL,
	H_KEY VARCHAR(255) NOT NULL,
	H_VALUE VARCHAR(255) NOT NULL
);

SELECT TOP 30 R_URL, R_METHOD, R_DATE, R_CONTROLLER, R_PARAMETERS, R_BODY, H_KEY, H_VALUE
FROM request INNER JOIN HEADER ON
request.R_ID = header.R_ID;

SELECT * FROM Request
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

INSERT INTO Request (R_URL,R_METHOD, R_CONTROLLER, R_PARAMETERS, R_BODY, R_DATE, R_CTYPE)
VALUES('{0}','{1}','{2}','{3}','{4}','03/27/1986','{6}');   
-- return id
SELECT CAST(scope_identity() AS int)
           