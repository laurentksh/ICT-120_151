import { TestBed } from '@angular/core/testing';

import { GlobalAppEventsService } from './global-app-events.service';

describe('GlobalAppEventsService', () => {
  let service: GlobalAppEventsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlobalAppEventsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
