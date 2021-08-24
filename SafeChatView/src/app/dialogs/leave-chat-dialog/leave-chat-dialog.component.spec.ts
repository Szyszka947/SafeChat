import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaveChatDialogComponent } from './leave-chat-dialog.component';

describe('LeaveChatDialogComponent', () => {
  let component: LeaveChatDialogComponent;
  let fixture: ComponentFixture<LeaveChatDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeaveChatDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaveChatDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
