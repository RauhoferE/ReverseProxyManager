export interface CertificateDto{
        id:number;
        name: string;

        issuer: string;

        subject: string;

        validNotBefore: Date;

        validNotAfter:Date;

        lastUpdated: Date;

        isUpToDate: boolean;

        serverEntity: IdNameDto | null;
}

export interface IdNameDto {
    id: number;
    name: string;
}