CREATE TABLE projects 
(
	projectId SERIAL PRIMARY KEY,
	title varchar(120) NOT NULL,
	mddescription text NOT NULL
);