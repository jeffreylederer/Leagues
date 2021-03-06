USE [master]
GO
/****** Object:  Database [Leagues]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE DATABASE [Leagues]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Leagues', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Leagues.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Leagues_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Leagues_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Leagues] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Leagues].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Leagues] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Leagues] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Leagues] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Leagues] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Leagues] SET ARITHABORT OFF 
GO
ALTER DATABASE [Leagues] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Leagues] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Leagues] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Leagues] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Leagues] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Leagues] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Leagues] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Leagues] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Leagues] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Leagues] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Leagues] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Leagues] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Leagues] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Leagues] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Leagues] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Leagues] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Leagues] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Leagues] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Leagues] SET  MULTI_USER 
GO
ALTER DATABASE [Leagues] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Leagues] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Leagues] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Leagues] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Leagues] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Leagues]
GO
/****** Object:  Table [dbo].[Player]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Player](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[TuesdayLeague] [bit] NOT NULL,
	[WednesdayLeague] [bit] NOT NULL,
	[FullName]  AS (([FirstName]+' ')+[Lastname]),
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TuesdayMatch]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TuesdayMatch](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[GameDate] [int] NOT NULL,
	[Rink] [int] NOT NULL,
	[Team1] [int] NOT NULL,
	[Team2] [int] NOT NULL,
 CONSTRAINT [PK_TuesdaySchedule] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TuesdaySchedule]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TuesdaySchedule](
	[id] [int] NOT NULL,
	[GameDate] [date] NOT NULL,
	[GameDateFormatted]  AS (CONVERT([varchar](12),[GameDate],(1))),
 CONSTRAINT [PK_TuesdaySchedule_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TuesdayTeam]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TuesdayTeam](
	[id] [int] NOT NULL,
	[Skip] [int] NOT NULL,
	[Lead] [int] NULL,
 CONSTRAINT [PK_Tuesday] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WednesdayMatch]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WednesdayMatch](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[GameDate] [int] NOT NULL,
	[Rink] [int] NOT NULL,
	[Team1] [int] NOT NULL,
	[Team2] [int] NOT NULL,
 CONSTRAINT [PK_WednesdayMatch] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WednesdaySchedule]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WednesdaySchedule](
	[id] [int] NOT NULL,
	[GameDate] [date] NOT NULL,
	[GameDateFormatted]  AS (CONVERT([varchar](12),[GameDate],(1))),
 CONSTRAINT [PK_WednesdaySchedule] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WednesdayTeam]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WednesdayTeam](
	[id] [int] NOT NULL,
	[Skip] [int] NOT NULL,
	[ViceSkip] [int] NULL,
	[Lead] [int] NULL,
 CONSTRAINT [PK_WednesdayTeam] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Player]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Player] ON [dbo].[Player]
(
	[LastName] ASC,
	[FirstName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TuesdayMatch]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_TuesdayMatch] ON [dbo].[TuesdayMatch]
(
	[GameDate] ASC,
	[Team1] ASC,
	[Team2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TuesdayTeam]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_TuesdayTeam] ON [dbo].[TuesdayTeam]
(
	[Lead] ASC,
	[Skip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TuesdayTeam_1]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_TuesdayTeam_1] ON [dbo].[TuesdayTeam]
(
	[Skip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_TuesdayTeam_2]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_TuesdayTeam_2] ON [dbo].[TuesdayTeam]
(
	[Skip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdayMatch]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_WednesdayMatch] ON [dbo].[WednesdayMatch]
(
	[GameDate] ASC,
	[Rink] ASC,
	[Team1] ASC,
	[Team2] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdaySchedule]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_WednesdaySchedule] ON [dbo].[WednesdaySchedule]
(
	[GameDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdayTeam]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_WednesdayTeam] ON [dbo].[WednesdayTeam]
(
	[Skip] ASC,
	[ViceSkip] ASC,
	[Lead] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdayTeam_1]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_WednesdayTeam_1] ON [dbo].[WednesdayTeam]
(
	[Skip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdayTeam_2]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_WednesdayTeam_2] ON [dbo].[WednesdayTeam]
(
	[ViceSkip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WednesdayTeam_3]    Script Date: 4/7/2019 11:18:20 AM ******/
CREATE NONCLUSTERED INDEX [IX_WednesdayTeam_3] ON [dbo].[WednesdayTeam]
(
	[Lead] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_TuesdayLeague]  DEFAULT ((0)) FOR [TuesdayLeague]
GO
ALTER TABLE [dbo].[Player] ADD  CONSTRAINT [DF_Player_WednesdayLeague]  DEFAULT ((0)) FOR [WednesdayLeague]
GO
ALTER TABLE [dbo].[TuesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_TuesdaySchedule_TuesdaySchedule] FOREIGN KEY([GameDate])
REFERENCES [dbo].[TuesdaySchedule] ([id])
GO
ALTER TABLE [dbo].[TuesdayMatch] CHECK CONSTRAINT [FK_TuesdaySchedule_TuesdaySchedule]
GO
ALTER TABLE [dbo].[TuesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_TuesdaySchedule_TuesdayTeam] FOREIGN KEY([Team1])
REFERENCES [dbo].[TuesdayTeam] ([id])
GO
ALTER TABLE [dbo].[TuesdayMatch] CHECK CONSTRAINT [FK_TuesdaySchedule_TuesdayTeam]
GO
ALTER TABLE [dbo].[TuesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_TuesdaySchedule_TuesdayTeam1] FOREIGN KEY([Team2])
REFERENCES [dbo].[TuesdayTeam] ([id])
GO
ALTER TABLE [dbo].[TuesdayMatch] CHECK CONSTRAINT [FK_TuesdaySchedule_TuesdayTeam1]
GO
ALTER TABLE [dbo].[TuesdayTeam]  WITH CHECK ADD  CONSTRAINT [FK_Tuesday_Players] FOREIGN KEY([Skip])
REFERENCES [dbo].[Player] ([id])
GO
ALTER TABLE [dbo].[TuesdayTeam] CHECK CONSTRAINT [FK_Tuesday_Players]
GO
ALTER TABLE [dbo].[TuesdayTeam]  WITH CHECK ADD  CONSTRAINT [FK_Tuesday_Players1] FOREIGN KEY([Lead])
REFERENCES [dbo].[Player] ([id])
GO
ALTER TABLE [dbo].[TuesdayTeam] CHECK CONSTRAINT [FK_Tuesday_Players1]
GO
ALTER TABLE [dbo].[WednesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayMatch_WednesdaySchedule] FOREIGN KEY([GameDate])
REFERENCES [dbo].[WednesdaySchedule] ([id])
GO
ALTER TABLE [dbo].[WednesdayMatch] CHECK CONSTRAINT [FK_WednesdayMatch_WednesdaySchedule]
GO
ALTER TABLE [dbo].[WednesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayMatch_WednesdayTeam] FOREIGN KEY([Team1])
REFERENCES [dbo].[WednesdayTeam] ([id])
GO
ALTER TABLE [dbo].[WednesdayMatch] CHECK CONSTRAINT [FK_WednesdayMatch_WednesdayTeam]
GO
ALTER TABLE [dbo].[WednesdayMatch]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayMatch_WednesdayTeam1] FOREIGN KEY([Team2])
REFERENCES [dbo].[WednesdayTeam] ([id])
GO
ALTER TABLE [dbo].[WednesdayMatch] CHECK CONSTRAINT [FK_WednesdayMatch_WednesdayTeam1]
GO
ALTER TABLE [dbo].[WednesdayTeam]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayTeam_Players] FOREIGN KEY([Skip])
REFERENCES [dbo].[Player] ([id])
GO
ALTER TABLE [dbo].[WednesdayTeam] CHECK CONSTRAINT [FK_WednesdayTeam_Players]
GO
ALTER TABLE [dbo].[WednesdayTeam]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayTeam_Players1] FOREIGN KEY([ViceSkip])
REFERENCES [dbo].[Player] ([id])
GO
ALTER TABLE [dbo].[WednesdayTeam] CHECK CONSTRAINT [FK_WednesdayTeam_Players1]
GO
ALTER TABLE [dbo].[WednesdayTeam]  WITH CHECK ADD  CONSTRAINT [FK_WednesdayTeam_Players2] FOREIGN KEY([Lead])
REFERENCES [dbo].[Player] ([id])
GO
ALTER TABLE [dbo].[WednesdayTeam] CHECK CONSTRAINT [FK_WednesdayTeam_Players2]
GO
/****** Object:  StoredProcedure [dbo].[CheckTuesdaySchedule]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CheckTuesdaySchedule]		
as

begin
create table ##tempresults
(
	results text not null
)

-- make sure all teams have players
insert into ##tempresults (results)
select 'missing players on team ' + cast(id as varchar(2)) from [dbo].[WednesdayTeam] where  lead is null

-- first make sure each player is only on one team
create table ##temp
(
	aplayer int not null
)
  -- get all the players
insert into ##temp (aplayer)
select t.skip from [dbo].[TuesdayTeam] T 
union 
select t.Lead from [dbo].[TuesdayTeam] T 

select distinct aplayer from ##temp where count(*) >2
insert into ##tempresults (results) 
select 'on more than one team: ' + p.FirstName + ' ' + p.LastName from ##temp t inner join player p on p.id = t.aplayer
drop table ##temp

-- second make sure each team plays other teams no more than once
insert into ##tempresults (results) 
select cast(T1.Team1 as varchar(2)) + ' vs ' + cast(T1.Team2 as varchar(2)) + ' on ' + convert(varchar, S1.GameDate,1)+ ' and ' + 
 cast(t2.Team1 as varchar(2)) +' vs ' + cast(t2.Team2 as varchar(2))+ ' on' +  convert(varchar, S2.GameDate,1) from [dbo].[TuesdayMatch] T1
inner join [dbo].[TuesdayMatch] T2 on T1.Team1=T2.Team2 and T1.Team2=T2.Team1
inner join [dbo].[TuesdaySchedule] S1 on S1.id=t1.GameDate
inner join [dbo].[TuesdaySchedule] S2 on S2.id=t2.GameDate
where t1.id <> t2.id


select * from ##tempresults
drop table ##tempresults
end


GO
/****** Object:  StoredProcedure [dbo].[CheckWednesdaySchedule]    Script Date: 4/7/2019 11:18:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CheckWednesdaySchedule]		
as

begin
create table ##tempresults
(
	results text not null
)

-- make sure all teams have players
select 'missing players on team ' + cast(id as varchar(2)) from [dbo].[WednesdayTeam] where ViceSkip is null or lead is null

-- first make sure each player is only on one team
create table ##temp
(
	aplayer int not null
)
  -- get all the players
insert into ##temp (aplayer)
select t.skip from [dbo].[WednesdayTeam] T 
union
select t.ViceSkip from [dbo].[WednesdayTeam] T 
union 
select t.Lead from [dbo].[WednesdayTeam] T 

select distinct aplayer from ##temp where count(*) >2
insert into ##tempresults (results) 
select 'on more than one team: ' + p.FirstName + ' ' + p.LastName from ##temp t inner join player p on p.id = t.aplayer
drop table ##temp

-- second make sure each team plays other teams no more than once
insert into ##tempresults (results) 
select cast(T1.Team1 as varchar(2)) + ' vs ' + cast(T1.Team2 as varchar(2)) + ' on ' + convert(varchar, S1.GameDate,1)+ ' and ' + 
 cast(t2.Team1 as varchar(2)) +' vs ' + cast(t2.Team2 as varchar(2))+ ' on' +  convert(varchar, S2.GameDate,1) from [dbo].[WednesdayMatch] T1
inner join [dbo].[WednesdayMatch]  T2 on T1.Team1=T2.Team2 and T1.Team2=T2.Team1
inner join [dbo].[WednesdaySchedule] S1 on S1.id=t1.GameDate
inner join [dbo].[WednesdaySchedule] S2 on S2.id=t2.GameDate


select * from ##tempresults
drop table ##tempresults
end


GO
USE [master]
GO
ALTER DATABASE [Leagues] SET  READ_WRITE 
GO
