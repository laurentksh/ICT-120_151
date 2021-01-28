import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreatedUserViewModel } from 'src/app/auth/models/created-user-view-model';
import { Login } from 'src/app/auth/models/login';
import { Signup } from 'src/app/auth/models/signup';
import { UserSession } from 'src/app/auth/models/user-session';
import { CreatePublication } from 'src/app/publication/models/create-publication';
import { Like } from 'src/app/publication/models/like';
import { Publication } from 'src/app/publication/models/publication';
import { Repost } from 'src/app/publication/models/repost';
import { UpdateUser } from 'src/app/user/models/update-user';
import { UserSummary } from 'src/app/user/models/user-summary';
import { ApiCallResult } from './models/api-call-result';

/**
 * Global service that allows easy access to all API Endpoints exposed by the .NET server.
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly BASE_URL: string = "https://localhost:5001/api/";

  constructor(private httpClient: HttpClient) { }

  //#region Feed
  public async GetMainFeed(amount: number, positionId: string): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Success = true;

    let params: any;
    
    if (positionId == null)
      params = {'amount': amount};
    else
      params = {'amount': amount, 'positionId': positionId};
    
    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + "Feed", { params: params}).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetFeed(amount: number, positionId: string, userId: string): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Success = true;

    let params: any;

    if (positionId == null)
      params = {'amount': amount};
    else
      params = {'amount': amount, 'positionId': positionId};
    
    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + `Feed/${userId}`, { params: params}).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }
  //#endregion Feed

  //#region Media
  public async GetImage(mediaId: string): Promise<ApiCallResult<Blob>> {
    let result: ApiCallResult<Blob> = {} as ApiCallResult<Blob>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get(this.BASE_URL + `Media/image/${mediaId}`, { responseType: "blob" }).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetVideo(mediaId: string): Promise<ApiCallResult<Blob>> {
    let result: ApiCallResult<Blob> = {} as ApiCallResult<Blob>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get(this.BASE_URL + `Media/video/${mediaId}`, { responseType: "blob" }).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }
  //#endregion Media

  //#region Publication
  public async GetPublication(publicationId: string): Promise<ApiCallResult<Publication>> {
    let result: ApiCallResult<Publication> = {} as ApiCallResult<Publication>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication>(this.BASE_URL + "Publication/").toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async DeletePublication(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetReplies(publicationId: string): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetReposts(publicationId: string): Promise<ApiCallResult<Repost[]>> {
    let result: ApiCallResult<Repost[]> = {} as ApiCallResult<Repost[]>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<Repost[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetLikes(publicationId: string): Promise<ApiCallResult<Like[]>> {
    let result: ApiCallResult<Like[]> = {} as ApiCallResult<Like[]>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<Like[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async CreateNewPublication(publicationDto: CreatePublication): Promise<ApiCallResult<Publication>> {
    let result: ApiCallResult<Publication> = {} as ApiCallResult<Publication>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.post<Publication>(this.BASE_URL + "Publication/new", publicationDto).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async Repost(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/repost", null).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async Like(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/like", null).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async UnRepost(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/repost", null).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async UnLike(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "Publication/like").toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }
  //#endregion Publication

  //#region User
  public async GetUserSummary(identifier: string): Promise<ApiCallResult<UserSummary>> {
    let result: ApiCallResult<UserSummary> = {} as ApiCallResult<UserSummary>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<UserSummary>(this.BASE_URL + `User/get/${identifier}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async Authenticate(login: Login): Promise<ApiCallResult<UserSession>> {
    let result: ApiCallResult<UserSession> = {} as ApiCallResult<UserSession>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.post<UserSession>(this.BASE_URL + "User/auth", login).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async GetSessions(): Promise<ApiCallResult<UserSession[]>> {
    let result: ApiCallResult<UserSession[]> = {} as ApiCallResult<UserSession[]>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.get<UserSession[]>(this.BASE_URL + "User/sessions").toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async DeleteSession(sessionId: string): Promise<ApiCallResult<void>> { //TODO: Test this
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/sessions/${sessionId}`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async DeleteSessions(allSessions: boolean): Promise<ApiCallResult<void>> { //TODO: Test this
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "User/sessions", { params: {"allSessions": allSessions.toString()}}).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async CreateNewUser(createDto: Signup): Promise<ApiCallResult<CreatedUserViewModel>> {
    let result: ApiCallResult<CreatedUserViewModel> = {} as ApiCallResult<CreatedUserViewModel>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.post<CreatedUserViewModel>(this.BASE_URL + "User/new", createDto).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async UpdateUser(userId: string, updateDto: UpdateUser): Promise<ApiCallResult<UserSummary>> {
    let result: ApiCallResult<UserSummary> = {} as ApiCallResult<UserSummary>;
    result.Success = true;

    try {
      result.ObjectResult = await this.httpClient.post<UserSummary>(this.BASE_URL + "User/update", updateDto).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async DeleteUser(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "User/").toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async Follow(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.post(this.BASE_URL + `User/${userId}/follow`, null).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async UnFollow(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/${userId}/follow`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async Block(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.post(this.BASE_URL + `User/${userId}/block`, null).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }

  public async UnBlock(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Success = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/${userId}/block`).toPromise();
    } catch (error) {
      result.Success = false;
      result.Error = error;
    }

    return result;
  }
  //#endregion User
}
