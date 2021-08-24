import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyInvitationsDialogComponent } from './my-invitations-dialog.component';

describe('MyInvitationsDialogComponent', () => {
  let component: MyInvitationsDialogComponent;
  let fixture: ComponentFixture<MyInvitationsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyInvitationsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyInvitationsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
