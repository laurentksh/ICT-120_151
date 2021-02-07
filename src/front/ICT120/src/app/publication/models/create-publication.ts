export interface CreatePublication {
    submissionType: SubmissionType;
    textContent: string;
    mediaId: string;
    replyPublicationId: string;
}

export enum SubmissionType {
    Text = 0,

    Image = 1,

    Video = 2,
    
    /**
     * A reply cannot be of this type.
     */
    Poll = 3
}