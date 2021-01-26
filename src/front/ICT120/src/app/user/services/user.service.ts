import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
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
}
