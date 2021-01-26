import { HttpErrorResponse } from "@angular/common/http";

export interface ApiCallResult<TResult> {
    Success: boolean;
    Error: HttpErrorResponse;
    ObjectResult: TResult;
}
