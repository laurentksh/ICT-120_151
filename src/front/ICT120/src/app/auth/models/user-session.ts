export interface UserSession {
    id: string;
    token: string;
    creationdate: Date;
    expiracydate: Date;
    userid: string;
}
