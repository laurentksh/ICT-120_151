export interface UserSummary {
    id: string;
    username: string;
    email: string;
    profilePictureId: string;
    biography: string;
    birthday: Date;
    creationDate: Date;
    following: boolean;
    followsMe: boolean;
    blocking: boolean;
    blocksMe: boolean;
}