import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HTMLLoadingAnimationComponent } from './htmlloading-animation.component';

describe('HTMLLoadingAnimationComponent', () => {
  let component: HTMLLoadingAnimationComponent;
  let fixture: ComponentFixture<HTMLLoadingAnimationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HTMLLoadingAnimationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HTMLLoadingAnimationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
