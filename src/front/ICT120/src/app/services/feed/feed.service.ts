import { Injectable } from '@angular/core';
import { Publication } from 'src/app/publication/models/publication';
import { ApiService } from '../api/api.service';
import { ApiCallResult } from '../api/models/api-call-result';
import { OperationResult } from '../api/models/operation-result';

@Injectable({
  providedIn: 'root'
})
export class FeedService { //TODO: Rethink this shit

  constructor(private apiService: ApiService) { }

  public async GetMainFeed(): Promise<OperationResult<Publication[]>> {
    let apiResult: ApiCallResult<Publication[]>;
    let result: OperationResult<Publication[]> = {} as OperationResult<Publication[]>;

    apiResult = await this.apiService.GetMainFeed();

    return result;
  }

  public async GetFeed(userId: string): Promise<OperationResult<Publication[]>> {
    let apiResult: ApiCallResult<Publication[]>;
    let result: OperationResult<Publication[]>;

    apiResult = await this.apiService.GetFeed(userId);

    result.result = apiResult.Result;
    result.content = apiResult.ObjectResult;
    result.errorMessage = apiResult.Exception.message;

    return result;
  }
}
