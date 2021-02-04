import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/services/api/api.service';
import { ApiCallResult } from 'src/app/services/api/models/api-call-result';
import { OperationResult } from 'src/app/services/models/operation-result';
import { UserSummary } from 'src/app/user/models/user-summary';
import { CreatedUserViewModel } from '../models/created-user-view-model';
import { Login } from '../models/login';
import { Signup } from '../models/signup';
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

  public get LocalUser(): UserSummary {
    const json = JSON.parse(localStorage.getItem("user"));
    let result: UserSummary = {} as UserSummary;

    result.id = json.id;
    result.username = json.username;
    result.biography = json.biography;
    result.creationDate = new Date(json.creationDate);
    result.birthday = new Date(json.birthday);
    result.profilePictureId = json.profilePictureId;

    return result;
  }

  public get SessionToken(): string {
    return this.Session.token;
  }

  private setSession(session: UserSession): void {
    localStorage.setItem("session", JSON.stringify(session));
  }

  private setUser(user: UserSummary): void {
    localStorage.setItem("user", JSON.stringify(user));
  }

  public DeleteSession(): void {
    localStorage.removeItem("session");
  }

  public DeleteUser(): void {
    localStorage.removeItem("user");
  }

  private async getLocalUserFromAPI(id: string): Promise<OperationResult<void>> {
    let result: OperationResult<void> = {} as OperationResult<void>;
    const request = await this.apiService.GetUserSummary(id);

    result.Success = request.Success;
    result.Error = request.Error;

    if (request.Success) {
      this.setUser(request.ObjectResult);
    }

    return result;
  }

  constructor(private apiService: ApiService) { }

  public async Signup(signup: Signup): Promise<OperationResult<UserSummary>> {
    let result: OperationResult<UserSummary> = {} as OperationResult<UserSummary>;
    const request = await this.apiService.CreateNewUser(signup);

    result.Success = request.Success;
    result.Error = request.Error;
    if (request.Success) {
      this.setSession(request.ObjectResult.session);
      this.setUser(request.ObjectResult.user);

      result.Content = request.ObjectResult.user;
    }

    return result;
  }

  public async Authenticate(login: Login): Promise<OperationResult<void>> {
    let result: OperationResult<void> = {} as OperationResult<void>;
    let request: ApiCallResult<UserSession>;

    request = await this.apiService.Authenticate(login);

    result.Success = request.Success;
    result.Error = request.Error;
    if (request.Success) {
      this.setSession(request.ObjectResult);

      await this.getLocalUserFromAPI(request.ObjectResult.userId);
    }

    return result;
  }

  public async Logout(): Promise<OperationResult<void>> {
    let result: OperationResult<void> = {} as OperationResult<void>;
    const request = await this.apiService.DeleteSession(this.Session.id);

    this.DeleteSession();

    result.Success = request.Success;
    result.Error = request.Error;
    return result;
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
