import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { OperationResult } from 'src/app/services/models/operation-result';
import { MediaViewModel } from '../models/media-view-model';

@Injectable({
  providedIn: 'root'
})
export class MediaService {

  constructor(private apiService: ApiService) { }

  public async GetMedia(id: string): Promise<OperationResult<MediaViewModel>> {
    let media: OperationResult<MediaViewModel> = {} as OperationResult<MediaViewModel>;

    const result = await this.apiService.GetMedia(id);

    media.Success = result.Success;
    if (result.Success) {
      media.Content = result.ObjectResult;
    } else {
      media.Error = result.Error;
    }

    return media;
  }

  public async GetBlob(url: string): Promise<OperationResult<Blob>> {
    let blob: OperationResult<Blob> = {} as OperationResult<Blob>;
    
    const result = await this.apiService.GetBlob(url);

    blob.Success = result.Success;
    if (result.Success) {
      blob.Content = result.ObjectResult;
    } else {
      blob.Error = result.Error;
    }

    return blob;
  }
}
