CREATE TABLE [dbo].[T_AutomationRunLog]
(

	[AutoName] nvarchar(200)  NOT NULL PRIMARY KEY,
	[AutoDescription] NVARCHAR(MAX) NULL ,
    [RunStatus] NVARCHAR(200) NULL, 
    [RunResult] NVARCHAR(200) NULL, 
    [LatestRunCompleteTime] DATETIME NULL, 
    [RunLog] NVARCHAR(MAX) NULL, 
    [RunDataFile] NVARCHAR(MAX) NULL
    
)
