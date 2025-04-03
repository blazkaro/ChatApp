import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvitationsListDialogComponent } from './invitations-list-dialog.component';

describe('InvitationsListDialogComponent', () => {
  let component: InvitationsListDialogComponent;
  let fixture: ComponentFixture<InvitationsListDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InvitationsListDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvitationsListDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
