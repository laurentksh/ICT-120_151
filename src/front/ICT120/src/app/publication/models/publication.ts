export interface Publication {
    Id: string;

    CreationDate: Date;

    SubmissionType: string;

    TextContent: string;

    MediaUrl: string;

    RepliesAmount: number;

    RetweetsAmount: number;

    LikesAmount: number;

    Liked: boolean;

    Reposted: boolean;

    User: string;

    ReplyPublicationId: string;
}
