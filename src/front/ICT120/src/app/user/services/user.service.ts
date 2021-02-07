import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
import { OperationResult } from 'src/app/services/models/operation-result';
import { UserSummary } from '../models/user-summary';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) { }

  public async GetUser(identifier: string): Promise<OperationResult<UserSummary>> {
    let result: OperationResult<UserSummary> = {} as OperationResult<UserSummary>;
    const request = await this.apiService.GetUserSummary(identifier);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async Follow(id: string, follow: boolean): Promise<OperationResult<void>> {
    const result: OperationResult<void> = {} as OperationResult<void>;
    let request: ApiCallResult<void> = null;

    if (follow)
      request = await this.apiService.Follow(id);
    else
      request = await this.apiService.UnFollow(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async Block(id: string, block: boolean): Promise<OperationResult<void>> {
    const result: OperationResult<void> = {} as OperationResult<void>;
    let request: ApiCallResult<void> = null;

    if (block)
      request = await this.apiService.Block(id);
    else
      request = await this.apiService.UnBlock(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }
}
