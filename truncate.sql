-- Delete from dependent tables
DELETE FROM HomeMemberNotification;
DELETE FROM Notification;
DELETE FROM HomeDevices;
DELETE FROM HomeMember;
DELETE FROM Sessions;
DELETE FROM Room;

-- Then delete from the main tables
DELETE FROM Devices;
DELETE FROM Homes;
DELETE FROM Companies;
DELETE FROM Users;
