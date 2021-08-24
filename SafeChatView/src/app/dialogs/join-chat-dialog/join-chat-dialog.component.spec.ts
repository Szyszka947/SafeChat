import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinChatDialogComponent } from './join-chat-dialog.component';

describe('JoinChatDialogComponent', () => {
  let component: JoinChatDialogComponent;
  let fixture: ComponentFixture<JoinChatDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JoinChatDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JoinChatDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
