import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
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
    result.creationdate = new Date(json.creationDate);
    result.expiracydate = new Date(json.expiracyDate);
    result.userid = json.userId;

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

  constructor(private apiService: ApiService) { }

  public async Authenticate(login: Login): Promise<[result: boolean, error: HttpErrorResponse]> {
    let result: ApiCallResult<UserSession>;

    result = await this.apiService.Authenticate(login);

    if (result.Result) {
      this.setSession(result.ObjectResult);
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
    const expiracyDate = session.expiracydate;

    if (expiracyDate <= now) {
      return false;
    }
    
    return true;
  }
}
