﻿CREATE TABLE [Users](
	[id] INTEGER PRIMARY KEY IDENTITY NOT NULL,
	[Name] varchar(50) NOT NULL,
)

CREATE TABLE [UserAnswers](
	[user_id] INTEGER NOT NULL,
	[answer_id] INTEGER NOT NULL,
	[id] INTEGER PRIMARY KEY IDENTITY NOT NULL
)


CREATE TABLE [Quiz](
	[id] INTEGER PRIMARY KEY IDENTITY NOT NULL,
	[Name] varchar(50) NULL
)

CREATE TABLE [Questions](
	[Id] INTEGER PRIMARY KEY IDENTITY NOT NULL,
	[SortOrder] INTEGER NULL,
	[Title] varchar(200) NULL,
	[quiz_id] INTEGER NOT NULL
)

CREATE TABLE [Answers](
	[id] INTEGER PRIMARY KEY IDENTITY NOT NULL,
	[question_id] INTEGER NOT NULL,
	[Text] varchar(200) NULL,
	[result_id] INTEGER NULL
)

CREATE TABLE [Results](
	[id] INTEGER PRIMARY KEY IDENTITY NOT NULL,
	[Title] varchar(50) NULL,
	[Text] varchar(500) NULL,
	[quiz_id] INTEGER NOT NULL
)