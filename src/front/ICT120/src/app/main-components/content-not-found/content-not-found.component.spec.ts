import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContentNotFoundComponent } from './content-not-found.component';

describe('ContentNotFoundComponent', () => {
  let component: ContentNotFoundComponent;
  let fixture: ComponentFixture<ContentNotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContentNotFoundComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContentNotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
