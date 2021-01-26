import { HttpErrorResponse } from "@angular/common/http";

export interface OperationResult<TResult> { //This interface is kinda useless but I wanted to separate the ApiService and other services.
    Success: boolean;
    Error: HttpErrorResponse;
    Content: TResult;
}
