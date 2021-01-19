import { TestBed } from '@angular/core/testing';

import { SessionTokenInterceptor } from './session-token.interceptor';

describe('SessionTokenInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      SessionTokenInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: SessionTokenInterceptor = TestBed.inject(SessionTokenInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
