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

http://housenet.ddns.net/web-api/index.php