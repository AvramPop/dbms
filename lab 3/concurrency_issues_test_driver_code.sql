use Casting
go

-- The concurrency issues are exemplified in this and concurrency_issues_use_cases file
-- the code from here should be run in parallel with the first and the second test case, the first failing and 
-- the second working.

-- Dirty read modification test function
-- When a transaction is allowed to read a row that has been modified by an another transaction which is not committed
select * from Movie
BEGIN TRANSACTION
UPDATE Movie SET
year=2021 WHERE id = 1
WAITFOR DELAY '00:00:10'
ROLLBACK TRANSACTION

--Non-repeatable read test function
-- A non-repeatable read occurs, when during the course of a transaction, a row is retrieved twice and the values within the row differ between reads.
INSERT INTO Movie(title, year) VALUES ('weird', 55)
BEGIN TRAN
WAITFOR DELAY '00:00:10'
UPDATE Movie SET year=1968 WHERE title ='weird'
COMMIT TRAN
delete from Movie where title = 'weird'

--Phantom read
-- A phantom read occurs when, in the course of a transaction, two identical queries are executed, and the collection of rows returned by the second query is different from the first.
BEGIN TRAN
WAITFOR DELAY '00:00:10'
INSERT INTO Movie(title,year) VALUES ('New nice movie for testing',5965)
COMMIT TRAN
delete from Movie where title = 'New nice movie for testing'

--Deadlock code for random killing
-- A SQL Server deadlock is a special concurrency problem in which two transactions block the progress of each other. The first transaction has a lock on some database object that the other transaction wants to access, and vice versa.
begin tran
update Movie set title='deadlock Movie Transaction 1' where id=9
waitfor delay '00:00:10'
update Actor set name='deadlock Actor Transaction 1' where id=4
commit tran

--Deadlock code for on purpose killing
begin tran
update Movie set title='deadlock Movie Transaction 1' where id=9
waitfor delay '00:00:10'
update Actor set name='deadlock Actor Transaction 1' where id=4
commit tran

-- Update conflict on optimistic level first batch
-- Optimistic concurrency do not lock a row when reading it. 
waitfor delay '00:00:10'
BEGIN TRAN
UPDATE Movie SET title =
'Israel' WHERE id=10;
waitfor delay '00:00:10'
COMMIT TRAN

--ALTER DATABASE Casting SET ALLOW_SNAPSHOT_ISOLATION ON
--ALTER DATABASE Casting SET READ_COMMITTED_SNAPSHOT ON

--update Movie set title = 'updating' where id = 10
--select * from Movie