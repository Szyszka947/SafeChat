import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KickUserDialogComponent } from './kick-user-dialog.component';

describe('KickUserDialogComponent', () => {
  let component: KickUserDialogComponent;
  let fixture: ComponentFixture<KickUserDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KickUserDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(KickUserDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
