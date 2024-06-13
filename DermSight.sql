--CREATE DATABASE DermSight

CREATE TABLE "User"(
	userId int IDENTITY,
	name NVARCHAR(10),
	account varchar(30),
	password varchar(100),
	role int,
)

CREATE TABLE News(
	newsId int IDENTITY,
	userId int,
	type nvarchar(10),
	title nvarchar(30),
	time datetime,
)

CREATE TABLE Symptom(
	symptomId int IDENTITY,
	diseaseId int,
	content nvarchar(Max)
)
CREATE TABLE Photo(
	photoId int IDENTITY,
	diseaseId int,
	route nvarchar(Max)
)

CREATE TABLE DiseaseRecord(
	recordId int IDENTITY,
	userId int,
	isCorrect bit,
	diseaseId int,
	time datetime,
)


CREATE TABLE RecordPhoto(
	photoId int IDENTITY,
	recordId int,
	route nvarchar(Max)
)