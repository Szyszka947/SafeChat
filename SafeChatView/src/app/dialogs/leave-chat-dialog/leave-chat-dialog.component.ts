import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { map } from 'rxjs/operators';
import { PublicGroupDto } from 'src/app/models/publicGroupDto';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-leave-chat-dialog',
  templateUrl: './leave-chat-dialog.component.html',
  styleUrls: ['./leave-chat-dialog.component.css']
})
export class LeaveChatDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) private data: PublicGroupDto, private _groupService: GroupService, private _toastService: ToastService,
    private _errorsHandlerService: ErrorsHandlerService, private dialogRef: MatDialogRef<LeaveChatDialogComponent>) { }

  groupName!: string;
  groupId!: number;

  ngOnInit(): void {
    this.groupName = this.data.groupName;
    this.groupId = this.data.id;
  }

  leaveGroup() {
    this._groupService.leaveGroup(this.groupId).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
      this.dialogRef.close();

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.leaveGroup();
      }

      const error = err.error['result'];
      this._toastService.toast('error', error['GroupId'] || error);
    });
  }

}
