-- Criando o Banco de Dados ConsoleSoccerApplication
CREATE DATABASE ConsoleSoccerApplication;
GO

-- Selecionando esse banco de dados
USE ConsoleSoccerApplication;
GO

-- Criando a tabela Competition 
CREATE TABLE Competition (
	ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ExternalCompetitionID INT NOT NULL, 
	CompetitionName NVARCHAR(150) NOT NULL,
	Code NVARCHAR(50) NOT NULL,
	Country NVARCHAR(200) NOT NULL,
	CreationDate DATETIME NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
);
GO

-- Criando a tabela Team 
CREATE TABLE Team (
	ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ExternalTeamID INT NOT NULL, 
	TeamName NVARCHAR(150) NOT NULL,
	ShortName NVARCHAR(70) NULL,
	TLA NVARCHAR(10) NOT NULL,
	CompetitionID INT FOREIGN KEY REFERENCES Competition(ID),
	CreationDate DATETIME NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
);
GO

-- Criando a tabela FavoriteTeam 
CREATE TABLE FavoriteTeam (
	ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	TeamID INT FOREIGN KEY REFERENCES Team(ID),
	CreationDate DATETIME NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
);
GO

-- Criando a tabela FavoritePlayer 
CREATE TABLE FavoritePlayer (
	ID INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	ExternalPlayerID INT NOT NULL,
	PlayerName NVARCHAR(150) NOT NULL,
	CreationDate DATETIME NOT NULL,
	Deleted BIT NOT NULL DEFAULT 0
);
GO