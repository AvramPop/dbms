--explanation in concurrency_issues_driver code

-- Dirty read test proof of bad use
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN

-- Dirty read test proof of good use
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN

--Non-repeatable read proof of bad use 
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN
--Non-repeatable read proof of good use 
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN
--Phantom read proof of bad use
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN
--Phantom read proof of good use
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN
SELECT * FROM Movie
WAITFOR DELAY '00:00:15'
SELECT * FROM Movie
COMMIT TRAN
--Deadlock code for random killing
begin tran
update Actor set name='deadlock Actor Transaction 2' where id=4
waitfor delay '00:00:10'
update Movie set title='deadlock Movie Transaction 2' where id=9
commit tran
--Deadlock code for on purpose killing
SET DEADLOCK_PRIORITY HIGH
begin tran
update Actor set name='deadlock Actor Transaction 2' where id=4
waitfor delay '00:00:10'
update Movie set title='deadlock Movie Transaction 2' where id=9
commit tran


-- Update conflict on optimistic level second batch
SET TRANSACTION ISOLATION LEVEL SNAPSHOT
BEGIN TRAN
Select * from Movie where id=10
Waitfor delay '00:00:10'
select * from Movie where id=4
Update Movie set year=3960 where id=10
COMMIT TRAN