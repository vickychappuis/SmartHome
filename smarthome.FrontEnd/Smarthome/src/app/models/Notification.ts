import { HomeMember } from "./home";

export type Notification = {
    notificationId: number;
    eventName: string;
    hardwareId: number;
    createdAt: string;
}

export type HomeMemberNotification = {
    notificationId: number;
    notification: Notification; 
    homeId: number;
    userId: number;
    homeMember: HomeMember;
    read: boolean;
  };
  