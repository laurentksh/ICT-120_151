import { UserSummary } from "src/app/user/models/user-summary";

export interface Publication {
    id: string;

    creationDate: string;

    submissionType: string;

    textContent: string;

    mediaId: string;

    repliesAmount: number;

    repostsAmount: number;

    likesAmount: number;

    liked: boolean;

    reposted: boolean;

    user: UserSummary;

    replyPublicationId: string;
}
