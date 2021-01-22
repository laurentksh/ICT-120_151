import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
import { UserSummary } from 'src/app/user/models/user-summary';
import { Login } from '../models/login';
import { UserSession } from '../models/user-session';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  public get IsAuthenticated() : boolean {
    return localStorage.getItem("session") != null;
  }

  public get Session(): UserSession {
    const json = JSON.parse(localStorage.getItem("session"));
    let result: UserSession = {} as UserSession;

    result.id = json.id;
    result.token = json.token;
    result.creationDateUtc = new Date(json.creationDate);
    result.expiracyDateUtc = new Date(json.expiracyDate);
    result.userId = json.userId;

    return result;
  }

  public get SessionToken(): string {
    return this.Session.token;
  }

  private setSession(session: UserSession): void {
    localStorage.setItem("session", JSON.stringify(session));
  }

  public DeleteSession(): void {
    localStorage.removeItem("session");
  }

  public get LocalUser(): UserSummary {
    const json = JSON.parse(localStorage.getItem("user"));
    let result: UserSummary = {} as UserSummary;

    result.Id = json.Id;
    result.Username = json.Username;
    result.Biography = json.Biography;
    result.CreationDate = new Date(json.CreationDate);
    result.Birthday = new Date(json.Birthday);
    result.ProfilePictureUrl = json.ProfilePictureUrl;

    return result;
  }

  private async getLocalUserFromAPI(id: string): Promise<[result: boolean, error: HttpErrorResponse]> {
    const result = await this.apiService.GetUserSummary(id);

    if (result.Result) {
      localStorage.setItem("user", JSON.stringify(result.ObjectResult));
      return [true, null];
    } else {
      return [false, result.Exception];
    }
  }

  constructor(private apiService: ApiService) { }

  public async Authenticate(login: Login): Promise<[result: boolean, error: HttpErrorResponse]> {
    let result: ApiCallResult<UserSession>;

    result = await this.apiService.Authenticate(login);

    if (result.Result) {
      this.setSession(result.ObjectResult);

      await this.getLocalUserFromAPI(result.ObjectResult.userId);
      return [true, null];
    } else {
      return [false, result.Exception];
    }
  }

  public async Logout(): Promise<[result: boolean, error: HttpErrorResponse]> {
    let result: ApiCallResult<void>;

    result = await this.apiService.DeleteSession(this.Session.id);

    this.DeleteSession();

    return [result.Result, result.Exception];
  }

  public ValidateCurrentSession(): boolean {
    const session = this.Session;
    const now = new Date(Date.now());
    const expiracyDate = session.expiracyDateUtc;

    if (expiracyDate <= now) {
      return false;
    }
    
    return true;
  }
}
