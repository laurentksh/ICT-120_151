import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { UserSummary } from '../models/user-summary';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private apiService: ApiService) { }

  public async GetUser(id: string): Promise<UserSummary> {
    let result = await this.apiService.GetUserSummary(id);

    if (result.Result) {}

    return null;
  }
}
