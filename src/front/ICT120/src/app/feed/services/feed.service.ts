import { Injectable } from '@angular/core';
import { Publication } from 'src/app/publication/models/publication';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
import { OperationResult } from 'src/app/services/models/operation-result';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  constructor(private apiService: ApiService) { }

  public async GetMainFeed(amount: number, positionId: string): Promise<OperationResult<Publication[]>> {
    let request: ApiCallResult<Publication[]> = await this.apiService.GetMainFeed(amount, positionId);
    let result: OperationResult<Publication[]> = {} as OperationResult<Publication[]>;

    if (request.Success) {
      result.Success = true;
      result.Content = request.ObjectResult;
    } else {
      result.Success = false;
      result.Error = result.Error;
    }

    return result;
  }

  public async GetFeed(amount: number, positionId: string, userId: string): Promise<OperationResult<Publication[]>> {
    let request: OperationResult<Publication[]> = {} as OperationResult<Publication[]>;
    const apiResult: ApiCallResult<Publication[]> = await this.apiService.GetFeed(amount, positionId, userId);

    request.Success = apiResult.Success;
    request.Content = apiResult.ObjectResult;
    request.Error = apiResult.Error;

    return request;
  }
}
