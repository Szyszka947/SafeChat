import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { map } from 'rxjs/operators';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-create-chat-dialog',
  templateUrl: './create-chat-dialog.component.html',
  styleUrls: ['./create-chat-dialog.component.css']
})
export class CreateChatDialogComponent implements OnInit {

  constructor(private _groupService: GroupService, private _toastService: ToastService, private _errorsHandlerService: ErrorsHandlerService,
    private dialogRef: MatDialogRef<CreateChatDialogComponent>) { }

  ngOnInit(): void {
  }

  formGroup: FormGroup = new FormGroup({
    ChatName: new FormControl(''),
    IsGroupPublic: new FormControl('')
  });

  createChat() {
    this._groupService.createGroup(this.formGroup.controls['ChatName'].value, this.formGroup.controls['IsGroupPublic'].value == "" ? false :
      this.formGroup.controls['IsGroupPublic'].value).pipe(map(data => data as any)).subscribe(data => {
        this._toastService.toast('success', data['result']);
        this.dialogRef.close();
      }, async err => {
        if (err.status == 401) {
          await this._errorsHandlerService.handle401StatusCode();
          return this.createChat();
        }
        const errorsInArray = err.error['result']['GroupName'];
        this._toastService.toast('error', errorsInArray == undefined ? err.error['result'] : errorsInArray[0]);
      });
  }

}
