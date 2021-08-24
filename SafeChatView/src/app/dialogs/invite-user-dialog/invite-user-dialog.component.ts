import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { UserDto } from 'src/app/models/userDto';
import { AccountService } from 'src/app/services/account.service';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-invite-user-dialog',
  templateUrl: './invite-user-dialog.component.html',
  styleUrls: ['./invite-user-dialog.component.css']
})
export class InviteUserDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) private groupId: number, private _accountService: AccountService, private _groupService: GroupService,
    private _toastService: ToastService, private _errorsHandlerService: ErrorsHandlerService, private dialogRef: MatDialogRef<InviteUserDialogComponent>) { }

  control: FormControl = new FormControl();
  selectedUserId!: number;
  usersList: Array<UserDto> = new Array<UserDto>();
  filteredUsersList: Observable<Array<UserDto>> | undefined;

  @ViewChild('userName')
  targetUserName!: ElementRef;

  ngOnInit(): void {
  }

  setFilteredUsersList() {
    this.filteredUsersList = this.control.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  private _filter(value: string): UserDto[] {
    const filterValue = value.toLowerCase();

    return this.usersList.filter(option => option.userName.toLowerCase().includes(filterValue));
  }

  updateSelectedOption(selectedUserId: string) {
    this.selectedUserId = parseInt(selectedUserId);
  }

  async searchUsers() {
    this.filteredUsersList = new Observable<Array<UserDto>>();
    this.usersList = new Array<UserDto>();
    this.usersList = await this._accountService.getUsersWhoNameStartsWith(this.targetUserName.nativeElement.value);
    this.setFilteredUsersList();
  }

  async inviteUser() {
    this._groupService.inviteToGroup(this.groupId, this.selectedUserId).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
      this.dialogRef.close();

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return await this.inviteUser();
      }

      const error = err.error['result'];

      this._toastService.toast('error', error['GroupId'] || error['InvitedUserId'] || error);
    });
  }

}
