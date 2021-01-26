import { UserSummary } from "src/app/user/models/user-summary";

export interface Publication {
    id: string;

    creationDate: Date;

    submissionType: string;

    textContent: string;

    mediaUrl: string;

    repliesAmount: number;

    repostsAmount: number;

    likesAmount: number;

    liked: boolean;

    reposted: boolean;

    user: UserSummary;

    replyPublicationId: string;
}
