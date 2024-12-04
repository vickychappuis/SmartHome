
export interface DeviceDto {
    Name: string;
    Model: string;
    Description: string;
    ImageUrl: string;

    CompanyId?: number;
    IsInterior?: boolean;
    IsExterior?: boolean;
    CanDetectMotion?: boolean;
    CanDetectPerson?: boolean;

    IsOpen?: boolean;
    
    IsTurnedOn?: boolean;

}