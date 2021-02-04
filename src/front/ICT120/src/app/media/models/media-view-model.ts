import { UserSummary } from "src/app/user/models/user-summary";

export interface MediaViewModel {
    id: string;
    mediaType: MediaType;
    mimeType: string;
    fileSize: number;
    blobFullUrl: string;
    owner: UserSummary;
}

export enum MediaType {
    Unknown = 0,
    Image = 1,
    Video = 2
}

export enum MediaContainer {
    Unknown = 0,
    Publication = 1,
    ProfilePicture = 2,
    PrivateMessage = 3
}