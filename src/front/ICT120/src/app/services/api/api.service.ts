import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Login } from 'src/app/auth/models/login';
import { UserSession } from 'src/app/auth/models/user-session';
import { CreatePublication } from 'src/app/publication/models/create-publication';
import { Like } from 'src/app/publication/models/like';
import { Publication } from 'src/app/publication/models/publication';
import { Repost } from 'src/app/publication/models/repost';
import { CreateUser } from 'src/app/user/models/create-user';
import { UpdateUser } from 'src/app/user/models/update-user';
import { User } from 'src/app/user/models/user';
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
  public async GetMainFeed(): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + "Feed").toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetFeed(userId: string): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + `Feed/${userId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }
  //#endregion Feed

  //#region Media
  public async GetImage(mediaId: string): Promise<ApiCallResult<Blob>> {
    let result: ApiCallResult<Blob> = {} as ApiCallResult<Blob>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get(this.BASE_URL + `Media/image/${mediaId}`, { responseType: "blob" }).toPromise()
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetVideo(mediaId: string): Promise<ApiCallResult<Blob>> {
    let result: ApiCallResult<Blob> = {} as ApiCallResult<Blob>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get(this.BASE_URL + `Media/video/${mediaId}`, { responseType: "blob" }).toPromise()
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }
  //#endregion Media

  //#region Publication
  public async GetPublication(publicationId: string): Promise<ApiCallResult<Publication>> {
    let result: ApiCallResult<Publication> = {} as ApiCallResult<Publication>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication>(this.BASE_URL + "Publication/").toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async DeletePublication(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetReplies(publicationId: string): Promise<ApiCallResult<Publication[]>> {
    let result: ApiCallResult<Publication[]> = {} as ApiCallResult<Publication[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Publication[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetReposts(publicationId: string): Promise<ApiCallResult<Repost[]>> {
    let result: ApiCallResult<Repost[]> = {} as ApiCallResult<Repost[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Repost[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetLikes(publicationId: string): Promise<ApiCallResult<Like[]>> {
    let result: ApiCallResult<Like[]> = {} as ApiCallResult<Like[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<Like[]>(this.BASE_URL + `Publication/${publicationId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async CreateNewPublication(publicationDto: CreatePublication): Promise<ApiCallResult<Publication>> {
    let result: ApiCallResult<Publication> = {} as ApiCallResult<Publication>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.post<Publication>(this.BASE_URL + "Publication/new", publicationDto).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async Repost(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/repost", null).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async Like(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/like", null).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async UnRepost(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.post(this.BASE_URL + "Publication/repost", null).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async UnLike(publicationId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "Publication/like").toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }
  //#endregion Publication

  //#region User
  public async GetUser(identifier: string): Promise<ApiCallResult<User>> {
    let result: ApiCallResult<User> = {} as ApiCallResult<User>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<User>(this.BASE_URL + `User/get/${identifier}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async Authenticate(login: Login): Promise<ApiCallResult<UserSession>> {
    let result: ApiCallResult<UserSession> = {} as ApiCallResult<UserSession>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.post<UserSession>(this.BASE_URL + "User/auth", login).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async GetSessions(): Promise<ApiCallResult<UserSession[]>> {
    let result: ApiCallResult<UserSession[]> = {} as ApiCallResult<UserSession[]>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.get<UserSession[]>(this.BASE_URL + "User/sessions").toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async DeleteSession(sessionId: string): Promise<ApiCallResult<void>> { //TODO: Test this
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/sessions/${sessionId}`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async DeleteSessions(allSessions: boolean): Promise<ApiCallResult<void>> { //TODO: Test this
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "User/sessions", { params: {"allSessions": allSessions.toString()}}).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async CreateNewUser(createDto: CreateUser): Promise<ApiCallResult<User>> {
    let result: ApiCallResult<User> = {} as ApiCallResult<User>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.post<User>(this.BASE_URL + "User/new", createDto).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async UpdateUser(userId: string, updateDto: UpdateUser): Promise<ApiCallResult<User>> {
    let result: ApiCallResult<User> = {} as ApiCallResult<User>;
    result.Result = true;

    try {
      result.ObjectResult = await this.httpClient.post<User>(this.BASE_URL + "User/update", updateDto).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async DeleteUser(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + "User/").toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async Follow(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.post(this.BASE_URL + `User/${userId}/follow`, null).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async UnFollow(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/${userId}/follow`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async Block(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.post(this.BASE_URL + `User/${userId}/block`, null).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }

  public async UnBlock(userId: string): Promise<ApiCallResult<void>> {
    let result: ApiCallResult<void> = {} as ApiCallResult<void>;
    result.Result = true;

    try {
      await this.httpClient.delete(this.BASE_URL + `User/${userId}/block`).toPromise();
    } catch (error) {
      result.Result = false;
      result.Exception = error;
    }

    return result;
  }
  //#endregion User
}

