import { HttpErrorResponse } from "@angular/common/http";

export interface ApiCallResult<TResult> {
    Result: boolean;
    Exception: HttpErrorResponse;
    ObjectResult: TResult;
}
