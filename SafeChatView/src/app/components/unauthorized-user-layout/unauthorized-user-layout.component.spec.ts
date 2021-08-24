import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnauthorizedUserLayoutComponent } from './unauthorized-user-layout.component';

describe('UnauthorizedUserLayoutComponent', () => {
  let component: UnauthorizedUserLayoutComponent;
  let fixture: ComponentFixture<UnauthorizedUserLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnauthorizedUserLayoutComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnauthorizedUserLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
