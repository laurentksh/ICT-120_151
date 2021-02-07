import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalAppEventsService {
  private loadingEventSource = new Subject<boolean>();
  private messageEventSource = new Subject<[value: boolean, msg: string, severity: MessageType]>();
  private snackbarEventSource = new Subject<[message: string, button: string, duration?: number]>();

  public loadingEvent = this.loadingEventSource.asObservable();
  public messageEvent = this.messageEventSource.asObservable();
  public snackbarEvent = this.snackbarEventSource.asObservable();

  constructor() { }

  public Loading(): void {
    this.loadingEventSource.next(true);
  }

  public DoneLoading(): void {
    this.loadingEventSource.next(false);
  }

  public ShowMessage(message: string, severity: MessageType): void {
    this.messageEventSource.next([true, message, severity]);
  }

  public HideMessage(): void {
    this.messageEventSource.next([false, null, null]);
  }

  public ShowSnackBarMessage(message: string, button?: string, duration?: number): void {
    this.snackbarEventSource.next([message, button, duration]);
  }
}

export enum MessageType {
  Information,
  Success,
  Warning,
  Error,
}
