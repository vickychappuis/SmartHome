SET IDENTITY_INSERT Users ON;

INSERT INTO Users (UserId, FirstName, LastName, Email, ImageUrl, Password, Role, RegisterDate)
VALUES
(1, 'John', 'Doe', 'john.doe@example.com', 'https://imageurl.com/johndoe.png', 'password123', 0, '2024-01-01'),
(2, 'Jane', 'Smith', 'jane.smith@example.com', 'https://imageurl.com/janesmith.png', 'password456', 1, '2024-01-05'),
(3, 'Carlos', 'Gonzalez', 'carlos.gonzalez@example.com', 'https://imageurl.com/carlosg.png', 'password789', 1, '2024-01-10'),
(4, 'Anna', 'Johnson', 'anna.johnson@example.com', 'https://imageurl.com/annaj.png', 'password321', 1, '2024-01-12'),
(5, 'Mark', 'Williams', 'mark.williams@example.com', 'https://imageurl.com/markw.png', 'password654', 1, '2024-01-15'),
(6, 'Sophia', 'Brown', 'sophia.brown@example.com', 'https://imageurl.com/sophiab.png', 'password987', 2, '2024-01-20'),
(7, 'Michael', 'Davis', 'michael.davis@example.com', 'https://imageurl.com/michaeld.png', 'password111', 2, '2024-01-22'),
(8, 'Emily', 'Martinez', 'emily.martinez@example.com', 'https://imageurl.com/emilym.png', 'password222', 2, '2024-01-25'),
(9, 'Daniel', 'Taylor', 'daniel.taylor@example.com', 'https://imageurl.com/danielt.png', 'password333', 2, '2024-01-28'),
(10, 'Isabella', 'Anderson', 'isabella.anderson@example.com', 'https://imageurl.com/isabellaa.png', 'password444', 2, '2024-01-30'),
(1024, 'Test', 'Administrator', 'testadmin@example.com', 'https://imageurl.com/testadmin.png', 'password1024', 0, '2024-01-01');

SET IDENTITY_INSERT Users OFF;
SET IDENTITY_INSERT Companies ON;

INSERT INTO Companies (CompanyId, Name, LogotypeUrl, Rut, CompanyOwnerId)
VALUES 
(3, 'Smart Home Inc.', 'https://logourl.com/smarthome.png', 987654321, 3),
(4, 'Urban Innovations', 'https://logourl.com/urbaninnovations.png', 456789012, 4),
(5, 'Cloudnet Services', 'https://logourl.com/cloudnet.png', 789012345, 5);

SET IDENTITY_INSERT Companies OFF;
SET IDENTITY_INSERT Devices ON;

INSERT INTO Devices ( DeviceId, Name, DeviceType, Model, Description, ImageUrl, CompanyId, IsInterior, IsExterior, CanDetectMotion, CanDetectPerson)
VALUES
    (4, 'Glass Break Sensor', 1, 'WS-310', 'Sensor to detect glass breakage', 'https://imageurl.com/windowsensor2.png', 3, 1, 0, 0, 0),
    (5, 'Smart Security Camera', 0, 'SC-220', 'Smart security camera with cloud storage', 'https://imageurl.com/securitycamera3.png', 3, 1, 0, 1, 1),
    (6, 'Window Motion Sensor', 1, 'WS-320', 'Advanced motion detection for windows', 'https://imageurl.com/windowsensor3.png', 3, 1, 0, 1, 0),
    (7, 'Panoramic Security Camera', 0, 'SC-230', 'Panoramic camera with full-room view', 'https://imageurl.com/securitycamera4.png', 4, 1, 0, 1, 1),
    (8, 'Simple Window Sensor', 1, 'WS-330', 'Basic window open/close sensor', 'https://imageurl.com/windowsensor4.png', 4, 1, 0, 0, 0),
    (9, 'Compact Security Camera', 0, 'SC-240', 'Compact indoor security camera', 'https://imageurl.com/securitycamera5.png', 5, 1, 0, 1, 0),
    (10, 'Double Window Sensor', 1, 'WS-340', 'Sensor for dual-window configurations', 'https://imageurl.com/windowsensor5.png', 5, 1, 0, 0, 0),
    (11, 'Door Sensor', 1, 'DS-400', 'Sensor to monitor door open/close status', 'https://imageurl.com/doorsensor1.png', 5, 1, 0, 0, 0);

SET IDENTITY_INSERT Devices OFF;
SET IDENTITY_INSERT Homes ON;

INSERT INTO Homes (HomeId, Address, Latitude, Longitude, MaxMembers)
VALUES
(1, '123 Main St', 40.7128, -74.0060, 5),  -- New York, NY
(2, '456 Oak Dr', 34.0522, -118.2437, 4);  -- Los Angeles, CA



INSERT INTO HomeMember (HomeId, UserId, CanAddDevice, CanListDevices, CanReceiveNotifications)
VALUES
(1, 6, 1, 1, 1),
(1, 7, 1, 0, 1),
(2, 8, 0, 1, 1);

SET IDENTITY_INSERT Homes OFF;
SET IDENTITY_INSERT HomeDevices ON;

INSERT INTO HomeDevices (HardwareId, HomeId, DeviceId, IsConnected, DeviceName)
VALUES
    (4, 1, 4, 1, 'Living Room Glass Sensor'),
    (5, 1, 5, 1, 'Backyard Camera'),
    (6, 2, 6, 1, 'Bedroom Window Sensor'),
    (7, 2, 7, 1, 'Garage Camera'),
    (8, 2, 8, 1, 'Patio Window Sensor'),
    (9, 2, 9, 1, 'Hallway Camera'),
    (10, 2, 10, 1, 'Guest Room Window Sensor');


SET IDENTITY_INSERT HomeDevices OFF;
SET IDENTITY_INSERT Room ON;

INSERT INTO Room (RoomId, RoomName, HomeId)
VALUES
    (1, 'Living Room', 1),
    (2, 'Kitchen', 1),
    (3, 'Master Bedroom', 1),
    (4, 'Bathroom', 1),
    (5, 'Garage', 1),
    (6, 'Living Room', 2),
    (7, 'Kitchen', 2),
    (8, 'Bedroom', 2),
    (9, 'Bathroom', 2),
    (10, 'Garage', 2);

SET IDENTITY_INSERT Room OFF;

SET IDENTITY_INSERT Notification ON;

INSERT INTO Notification (NotificationId, EventName, HardwareId, CreatedAt)
VALUES
(1, 'Motion Detected by Security Camera', 4, '2024-10-01 12:00:00'),
(2, 'Person Detected by Security Camera', 5, '2024-10-03 15:15:00'),
(3, 'Window Opened', 6, '2024-10-05 09:30:00'),
(4, 'Glass Break Detected', 7, '2024-10-07 22:45:00'),
(5, 'Motion Detected by Security Camera', 8, '2024-10-09 13:00:00'),
(6, 'Window Opened', 9, '2024-10-11 08:15:00'),
(7, 'Person Detected by Security Camera', 10, '2024-10-13 19:30:00'),
(8, 'Device Disconnected', 4, '2024-10-15 14:00:00'),
(9, 'Low Battery Warning', 5, '2024-10-17 11:15:00'),
(10, 'Multiple Motion Events', 7, '2024-10-19 16:30:00'),
(11, 'Window Closed', 6, '2024-10-21 10:45:00'),
(12, 'Device Reconnected', 4, '2024-10-23 15:00:00'),
(13, 'Suspicious Activity Detected', 5, '2024-10-24 20:15:00'),
(14, 'Person Detected at Night', 7, '2024-10-25 23:30:00'),
(15, 'Multiple People Detected', 9, '2024-10-27 01:15:00'),
(16, 'Continuous Motion for 5 Minutes', 10, '2024-10-28 02:30:00'),
(17, 'Glass Break Alert - High Confidence', 4, '2024-10-29 03:45:00'),
(18, 'Device Maintenance Required', 8, '2024-10-30 08:00:00');

SET IDENTITY_INSERT Notification OFF;

INSERT INTO HomeMemberNotification (HomeId, UserId, NotificationId, "Read")
VALUES
(1, 6, 1, 1),
(1, 7, 1, 0),
(2, 8, 6, 1),
(2, 8, 7, 0),
(1, 6, 2, 1),
(1, 7, 2, 1),
(1, 6, 8, 0),
(1, 7, 8, 0),
(1, 6, 9, 1),
(1, 7, 9, 0),
(1, 6, 12, 1),
(1, 7, 12, 1),
(1, 6, 13, 0),
(1, 7, 13, 0),
(1, 6, 17, 1),
(1, 7, 17, 0),
(2, 8, 3, 1),
(2, 8, 10, 0),
(2, 8, 11, 1),
(2, 8, 14, 0),
(2, 8, 15, 0),
(2, 8, 16, 1),
(2, 8, 18, 0);
