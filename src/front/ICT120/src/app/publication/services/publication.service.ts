import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { CreatePublication } from '../models/create-publication';
import { Publication } from '../models/publication';

@Injectable({
  providedIn: 'root'
})
export class PublicationService {

  constructor(private apiService: ApiService) { }

  public async CreateNew(dto: CreatePublication): Promise<OperationResult<Publication>> {
    let result: OperationResult<Publication> = {} as OperationResult<Publication>;

    const request = await this.apiService.CreateNewPublication(dto);

    if (request.Success) {
      result.Success = true;
      result.Content = request.ObjectResult;
    } else {
      result.Success = false;
      result.Error = request.Error;
    }

    return result;
  }

  public async GetPublication(id: string): Promise<OperationResult<Publication>> {
    let result: OperationResult<Publication> = {} as OperationResult<Publication>;

    const request = await this.apiService.GetPublication(id);

    if (request.Success) {
      result.Success = true;
      result.Content = request.ObjectResult;
    } else {
      result.Success = false;
      result.Error = request.Error;
    }

    return result;
  }

  public async GetReplies(id: string): Promise<OperationResult<Publication[]>> {
    let result: OperationResult<Publication[]> = {} as OperationResult<Publication[]>;

    const request = await this.apiService.GetReplies(id);

    if (request.Success) {
      result.Success = true;
      result.Content = request.ObjectResult;
    } else {
      result.Success = false;
      result.Error = request.Error;
    }

    return result;
  }

  public async Repost(id: string, like: boolean): Promise<OperationResult<void>> {
    let result: OperationResult<void> = {} as OperationResult<void>;

    let request = null;

    if (like)
      request = await this.apiService.Repost(id);
    else
      request = await this.apiService.UnRepost(id);

    if (request.Success) {
      result.Success = true;
    } else {
      result.Success = false;
      result.Error = request.Error;
    }

    return result;
  }

  public async Like(id: string, like: boolean): Promise<OperationResult<void>> {
    let result: OperationResult<void> = {} as OperationResult<void>;

    let request = null;

    if (like)
      request = await this.apiService.Like(id);
    else
      request = await this.apiService.UnLike(id);

    if (request.Success) {
      result.Success = true;
    } else {
      result.Success = false;
      result.Error = request.Error;
    }

    return result;
  }
}
