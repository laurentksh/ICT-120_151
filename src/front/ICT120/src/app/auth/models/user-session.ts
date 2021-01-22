export interface UserSession {
    id: string;
    token: string;
    creationDateUtc: Date;
    expiracyDateUtc: Date;
    userId: string;
}
