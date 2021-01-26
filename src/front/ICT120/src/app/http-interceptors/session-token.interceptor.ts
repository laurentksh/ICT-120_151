import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/services/auth.service';

@Injectable()
export class SessionTokenInterceptor implements HttpInterceptor {
  private readonly SessionTokenName: string = "X-ICT-151-TOKEN";

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!request.headers.has(this.SessionTokenName) && this.authService.IsAuthenticated) {
      request = request.clone({
        setHeaders: {
          'X-ICT-151-TOKEN': `${this.authService.SessionToken}`
        }
      });

      //request.headers.append(this.SessionTokenName, this.authService.SessionToken);
    }

    return next.handle(request);
  }
}
