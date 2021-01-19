export interface OperationResult<TResult> { //This interface is kinda useless but I wanted to separate the ApiService and other services.
    result: boolean;
    errorMessage: string;
    content: TResult;
}
