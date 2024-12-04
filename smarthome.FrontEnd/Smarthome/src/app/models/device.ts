export enum DeviceType {
    SecurityCamera = 0,
    WindowSensor = 1,
    SmartLamp = 2,
    MotionSensor = 3
}

export const DeviceTypeNames: Record<DeviceType, string> = {
    [DeviceType.SecurityCamera]: 'Security Camera',
    [DeviceType.WindowSensor]: 'Window Sensor',
    [DeviceType.SmartLamp]: 'Smart Lamp',
    [DeviceType.MotionSensor]: 'Motion Sensor'
}



export interface Device {
    deviceId: number;
    name: string;
    deviceType: DeviceType;
    model: string;
    description: string;
    imageUrl: string;
    companyId: number;

    isInterior?: boolean;
    isExterior?: boolean;
    canDetectMotion?: boolean;
    canDetectPerson?: boolean;

    isWindowOpen?: boolean;

    isTurnedOn?: boolean;
}