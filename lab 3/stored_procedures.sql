use Casting
go
--db creation
if object_id ('dbo.Movie', 'U') IS NOT NULL drop table dbo.Movie
if object_id ('dbo.Actor', 'U') IS NOT NULL drop table dbo.Actor
if object_id ('dbo.Distribution', 'U') IS NOT NULL drop table dbo.Distribution

create table Movie(
	id int primary key identity(1, 1),
	title varchar(100),
	year int
);

create table Actor(
	id int primary key identity(1, 1),
	name varchar(100));

create table Distribution(
	mid int foreign key references Movie(id),
	aid int foreign key references Actor(id),
	constraint pk_Distribution primary key (mid, aid)
);

go
--validation functions
create function validateMovieTitle (@title varchar(100)) returns int as
begin
	declare @return int
	set @return = 0
	if(@title in (select title from Movie))
		set @return = 1
	return @return
end
go

create function validateActorName (@name varchar(100)) returns int as
begin
	declare @return int
	set @return = 0
	if(@name in (select name from Actor))
		set @return = 1
	return @return
end
go
--req 1: sp with rollback
--inserts with exception throwing
create procedure addDistribution @actorName varchar(100), @movieTitle varchar(100)
as
	print 'inserting ' + @actorName + ' into ' + @movieTitle + ' cast'
	declare @movieId int
	declare @actorId int
	if(dbo.validateMovieTitle(@movieTitle) = 1)
	begin
		select @movieId = id from Movie where title = @movieTitle
		if(dbo.validateActorName(@actorName) = 1)
		begin
			select @actorId = id from Actor where name = @actorName
			insert into Distribution values (@movieId, @actorId)
		end
		else 
		begin
			raiserror('EXCEPTION: could not add distribution', 14, 1)
		end
	end
	else
	begin
		raiserror ('EXCEPTION: could not add distribution', 14, 1)
	end
go
create procedure addMovie @movieTitle varchar(100), @year int
as
	print 'inserting ' + @movieTitle
	if(dbo.validateMovieTitle(@movieTitle) = 0)
	begin
		insert into Movie values (@movieTitle, @year)
	end
	else
	begin
		raiserror ('EXCEPTION: could not add movie', 14, 1)
	end
go
create procedure addActor @actorName varchar(100)
as
	print 'inserting ' + @actorName
	if(dbo.validateActorName(@actorName) = 0)
	begin
		insert into Actor values (@actorName)
	end
	else
	begin
		raiserror ('EXCEPTION: could not add actor', 14, 1)
	end
go

-- the SP: if any of the small ones throw exception, everything is rollbacked
create procedure addDistributionFullTransaction @actorName varchar(100), @movieTitle varchar(100), @year int as
begin
	begin transaction
	begin try
		exec addMovie @movieTitle, @year
		exec addActor @actorName
		exec addDistribution @actorName, @movieTitle

		commit tran
		print 'successfull transaction adding movie and actor'
	end try
	begin catch
		rollback tran
		print ERROR_MESSAGE() 
		print 'transaction failed. rollbacking'
	end catch
end
-- the use cases
exec addDistributionFullTransaction @actorName = 'dani pop', @movieTitle = 'my moviiie', @year = 2020 -- this SP crashed due to the fact that there is already a 'dani pop' in the db
exec addDistributionFullTransaction @actorName = 'Moni Obo', @movieTitle = 'some movie', @year = 2020 -- this SP correctly adds the distribution

-- req 2: do as much as you can
-- each insert is wrapped into a transaction
go
create procedure addDistributionTransactional @actorName varchar(100), @movieTitle varchar(100)
as
	SET NOCOUNT ON
	declare @movieId int
	declare @actorId int
	BEGIN TRAN
	if(dbo.validateMovieTitle(@movieTitle) = 1)
	begin
		select @movieId = id from Movie where title = @movieTitle

		if(dbo.validateActorName(@actorName) = 1)
		begin
			select @actorId = id from Actor where name = @actorName
			insert into Distribution values (@movieId, @actorId)
			IF @@ROWCOUNT <> 1
			BEGIN
				PRINT 'Adding distribution has issues, rolling back tran. UNSUCCESSFUL'
				ROLLBACK TRAN
			END
	
			ELSE
			BEGIN
				COMMIT TRANSACTION
				print 'SUCCESSFULY added distribution'
			END
		end
		else 
		begin 
			print 'could not add distribution. UNSUCCESSFUL'
		end
	end

	else 
	begin
		print 'could not add distribution. UNSUCCESSFUL'
	end
go

go
create procedure addMovieTransactional @movieTitle varchar(100), @year int
as
	SET NOCOUNT ON
	BEGIN TRAN
	if(dbo.validateMovieTitle(@movieTitle) = 0)
	begin
		insert into Movie values (@movieTitle, @year)
		IF @@ROWCOUNT <> 1
		BEGIN
			PRINT 'Adding movie has issues, rolling back tran. UNSUCCESSFUL'
			ROLLBACK TRAN
		END
	
		ELSE
		BEGIN
			COMMIT TRANSACTION
			print 'SUCCESSFULY added movie'
		END
	end

	else 
	begin
		print 'could not add movie. UNSUCCESSFUL'
	end
go

go
create procedure addActorTransactional @actorName varchar(100)
as
	SET NOCOUNT ON
	BEGIN TRAN
	if(dbo.validateActorName(@actorName) = 0)
	begin
		insert into Actor values (@actorName)
		IF @@ROWCOUNT <> 1
		BEGIN
			PRINT 'Adding author has issues, rolling back tran. UNSUCCESSFUL'
			ROLLBACK TRAN
		END
	
		ELSE
		BEGIN
			COMMIT TRANSACTION
			print 'SUCCESSFULY added author'
		END
	end

	else 
	begin
		print 'could not add author. UNSUCCESSFUL'
	end
go
-- the final SP. does everything atomically
create procedure addDistributionEachTransaction @actorName varchar(100), @movieTitle varchar(100), @year int as
begin
	exec addActorTransactional @actorName
	exec addMovieTransactional @movieTitle, @year
	exec addDistributionTransactional @actorName, @movieTitle
end
-- the use cases 
exec addDistributionEachTransaction @actorName = 'unic', @movieTitle = 'some movie title', @year = 1111 -- this SP should crash on author and add only the movie and the distribution
exec addDistributionEachTransaction @actorName = 'new actor', @movieTitle = 'new muvie', @year = 1111 -- this SP should sucess
