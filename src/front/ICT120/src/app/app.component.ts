import { ChangeDetectorRef, Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavigationEnd, NavigationStart, Router, RouterEvent } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from './auth/services/auth.service';
import { GlobalAppEventsService, MessageType } from './services/global-app-events/global-app-events.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ICT-120';
  private loadingSubscription: Subscription;
  private msgSubscription: Subscription;
  private snackbarSubscription: Subscription;
  private routerSubscription: Subscription;

  public isLoading: boolean = false;
  public showMessage: boolean = false;
  public message: string = "";
  public severity: MessageType = MessageType.Information;

  constructor(
    private router: Router,
    private authService: AuthService,
    private globalAppEvents: GlobalAppEventsService,
    private snackBarService: MatSnackBar,
    private cdRef: ChangeDetectorRef
    ) {
    this.routerSubscription = router.events.subscribe((routerEvent: RouterEvent) => {
      this.handleRouterEvent(routerEvent);
    });
  }

  ngOnInit(): void {
    this.loadingSubscription = this.globalAppEvents.loadingEvent.subscribe(x => {
      this.isLoading = x;
      this.cdRef.detectChanges();
    });

    this.msgSubscription = this.globalAppEvents.messageEvent.subscribe(x => {
      this.showMessage = x[0];
      this.message = x[1];
      this.severity = x[2];
      this.cdRef.detectChanges();
    });

    this.snackbarSubscription = this.globalAppEvents.snackbarEvent.subscribe(x => {
      if (x[2] != null)
        this.snackBarService.open(x[0], x[1] ?? "Ok", { duration: x[2]})
      else
        this.snackBarService.open(x[0], x[1] ?? "Ok"); // Use default duration
    });
  }

  private handleRouterEvent(routerEvent: RouterEvent): void {
    if (routerEvent instanceof NavigationStart) {
      this.globalAppEvents.HideMessage();

      if (!this.authService.ValidateCurrentSession() && !routerEvent.url.startsWith("/login")) {
        console.log(routerEvent.url);
        this.authService.DeleteSession();
        this.router.navigate(["/login"], { queryParams: { redirect: routerEvent.url }});
      }
    }

    if (routerEvent instanceof NavigationEnd) {
      //this.globalAppEvents.DoneLoading();
    }
  }

  ngOnDestroy() {
    // Prevent memory leaks when the component is destroyed.
    this.loadingSubscription.unsubscribe();
    this.msgSubscription.unsubscribe();
    this.routerSubscription.unsubscribe();
    this.snackbarSubscription.unsubscribe();
  }
}
