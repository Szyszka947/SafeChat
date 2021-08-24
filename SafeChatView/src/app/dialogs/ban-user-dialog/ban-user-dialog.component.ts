import { Component, Inject, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { UserDto } from 'src/app/models/userDto';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-ban-user-dialog',
  templateUrl: './ban-user-dialog.component.html',
  styleUrls: ['./ban-user-dialog.component.css']
})
export class BanUserDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) private groupId: number, private dialogRef: MatDialogRef<BanUserDialogComponent>, private _groupService: GroupService,
    private _errorsHandlerService: ErrorsHandlerService, private _toastService: ToastService) { }

  groupMembersList: Array<UserDto> = new Array<UserDto>();
  filteredGroupMembersList: Observable<Array<UserDto>> | undefined;
  control: FormControl = new FormControl();
  selectedUserId!: number;
  selectedUserName!: string;
  banReasonControl: FormControl = new FormControl();
  banForDaysControl: FormControl = new FormControl();

  ngOnInit(): void {
    this.loadGroupMembers();
  }

  updateSelectedOption(selectedUserId: string, selectedUserName: string) {
    this.selectedUserId = parseInt(selectedUserId);
    this.selectedUserName = selectedUserName;
  }

  loadGroupMembers() {
    this._groupService.getGroupMembers(this.groupId).pipe(map(data => data as Array<UserDto>)).subscribe(data => {
      this.groupMembersList = data;

      this.filteredGroupMembersList = this.control.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.loadGroupMembers();
      }
    });
  }

  private _filter(value: string): UserDto[] {
    const filterValue = value.toLowerCase();

    return this.groupMembersList.filter(option => option.userName.toLowerCase().includes(filterValue));
  }

  banUser() {
    console.log(this.banForDaysControl.value);

    this._groupService.banUser(this.groupId, this.selectedUserId, this.banReasonControl.value, this.banForDaysControl.value).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
      this.dialogRef.close();

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.banUser();
      }

      var error = err.error['result'];
      this._toastService.toast('error', error['UserToBanId'] || error['GroupId'] || error['BanForDays'] || error);
    });
  }

}
