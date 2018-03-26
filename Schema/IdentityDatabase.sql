CREATE TABLE Identity (
	Identifier		varchar(16),
	DisplayName		varchar(255),

	Email			varchar(255),
	Created			datetime,
	Updated			datetime,

	PRIMARY KEY (Identifier)
)