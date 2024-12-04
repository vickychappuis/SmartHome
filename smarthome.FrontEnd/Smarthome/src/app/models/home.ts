export type Home = {
  homeId: number;
  address: string;
  latitude: number;
  longitude: number;
  maxMembers: number;
  homeName?: string;
};

export type HomeMember = {
  userId: number;
  firstName: string;
  email: string;
  canAddDevice: boolean;
  canListDevices: boolean;
  canReceiveNotifications: boolean;
  imageUrl: string;
};

export type HomeDevice = {
  name: string;
  model: string;
  description: string;
  roomId?: number;
  isConnected: boolean;
  imageUrl: string;
  isInterior?: boolean;
  isExterior?: boolean;
  canDetectMotion?: boolean;
  canDetectPerson?: boolean;
};

export type Room = {
  roomId: number;
  roomName: string;
  homeId: number;
};
