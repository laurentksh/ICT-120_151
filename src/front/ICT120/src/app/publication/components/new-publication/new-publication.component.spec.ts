import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPublicationComponent } from './new-publication.component';

describe('NewPublicationComponent', () => {
  let component: NewPublicationComponent;
  let fixture: ComponentFixture<NewPublicationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewPublicationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPublicationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
