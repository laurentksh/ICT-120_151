import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
import { OperationResult } from 'src/app/services/models/operation-result';
import { CreatePublication } from '../models/create-publication';
import { Publication } from '../models/publication';

@Injectable({
  providedIn: 'root'
})
export class PublicationService {

  constructor(private apiService: ApiService) { }

  public async CreateNew(dto: CreatePublication): Promise<OperationResult<Publication>> {
    const result: OperationResult<Publication> = {} as OperationResult<Publication>;

    const request = await this.apiService.CreateNewPublication(dto);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async GetPublication(id: string): Promise<OperationResult<Publication>> {
    const result: OperationResult<Publication> = {} as OperationResult<Publication>;

    const request = await this.apiService.GetPublication(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async GetReplies(id: string): Promise<OperationResult<Publication[]>> {
    const result: OperationResult<Publication[]> = {} as OperationResult<Publication[]>;

    const request = await this.apiService.GetReplies(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async Repost(id: string, repost: boolean): Promise<OperationResult<void>> {
    const result: OperationResult<void> = {} as OperationResult<void>;

    let request: ApiCallResult<void> = null;

    if (repost)
      request = await this.apiService.Repost(id);
    else
      request = await this.apiService.UnRepost(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async Like(id: string, like: boolean): Promise<OperationResult<void>> {
    const result: OperationResult<void> = {} as OperationResult<void>;

    let request: ApiCallResult<void> = null;

    if (like)
      request = await this.apiService.Like(id);
    else
      request = await this.apiService.UnLike(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }

  public async Delete(id: string): Promise<OperationResult<void>> {
    const result: OperationResult<void> = {} as OperationResult<void>;

    const request = await this.apiService.DeletePublication(id);

    result.Success = request.Success;
    if (request.Success) {
      result.Content = request.ObjectResult;
    } else {
      result.Error = request.Error;
    }

    return result;
  }
}
