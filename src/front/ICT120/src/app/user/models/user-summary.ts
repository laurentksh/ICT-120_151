export interface UserSummary {
    id: string
    username: string
    profilePictureId: string
    biography: string
    birthday: Date
    creationDate: Date
    following: boolean;
    followsMe: boolean;
    blocking: boolean;
    blocksMe: boolean;
}