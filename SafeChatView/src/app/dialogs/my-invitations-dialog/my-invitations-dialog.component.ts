import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { map } from 'rxjs/operators';
import { InvitationDto } from 'src/app/models/invitationDto';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-my-invitations-dialog',
  templateUrl: './my-invitations-dialog.component.html',
  styleUrls: ['./my-invitations-dialog.component.css']
})
export class MyInvitationsDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) private groupInvitations: Array<InvitationDto>, private _groupService: GroupService, private _toastService: ToastService,
    private _errorsHandlerService: ErrorsHandlerService) { }

  groupInvitationsList: Array<InvitationDto> = new Array<InvitationDto>();

  ngOnInit(): void {
    this.groupInvitationsList = this.groupInvitations;
  }

  acceptInvitation(groupId: number) {
    this._groupService.acceptGroupInvitation(groupId).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.acceptInvitation(groupId);
      }

      this._toastService.toast('error', err.error['result']['groupId'] || err.error['result']);
    });
  }

  discardInvitation(groupId: number) {
    this._groupService.discardGroupInvitation(groupId).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.discardInvitation(groupId);
      }

      this._toastService.toast('error', err.error['result']['groupId'] || err.error['result']);
    });
  }
}
