CREATE TABLE users 
(
	userId SERIAL PRIMARY KEY,
	firstName varchar(30) NOT NULL,
	lastName varchar(30) NOT NULL,
	patronymic varchar(30),
	usernameVk vkusername UNIQUE,
	email emailaddress UNIQUE,
	usernameTg tgusername UNIQUE,
	phoneNumber phonenumberru UNIQUE
);