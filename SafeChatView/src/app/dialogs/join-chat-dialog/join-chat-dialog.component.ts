import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatDialogRef } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { PublicGroupDto } from 'src/app/models/publicGroupDto';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { GroupService } from 'src/app/services/group.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-join-chat-dialog',
  templateUrl: './join-chat-dialog.component.html',
  styleUrls: ['./join-chat-dialog.component.css']
})
export class JoinChatDialogComponent implements OnInit {

  constructor(private _groupService: GroupService, private _errorsHandlerService: ErrorsHandlerService, private _toastService: ToastService,
    private dialogRef: MatDialogRef<JoinChatDialogComponent>) { }

  publicGroupsList: Array<PublicGroupDto> = new Array<PublicGroupDto>();
  control: FormControl = new FormControl();
  filteredGroupsList: Observable<Array<PublicGroupDto>> | undefined;
  selectedGroupId!: number;
  selectedGroupName!: string;

  getAllPublicGroups() {
    this._groupService.getAllPublicGroups().pipe(map(data => data as Array<PublicGroupDto>)).subscribe(data => {
      this.publicGroupsList = data;

      this.filteredGroupsList = this.control.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.getAllPublicGroups();
      }
    });
  }

  ngOnInit(): void {
    this.getAllPublicGroups();
  }

  private _filter(value: string): PublicGroupDto[] {
    const filterValue = value.toLowerCase();

    return this.publicGroupsList.filter(option => option.groupName.toLowerCase().includes(filterValue));
  }

  updateSelectedOption(selectedGroupId: string, selectedGroupName: string) {
    this.selectedGroupId = parseInt(selectedGroupId);
    this.selectedGroupName = selectedGroupName;
  }

  joinGroup() {
    this._groupService.joinGroup(this.selectedGroupId).pipe(map(data => data as any)).subscribe(data => {
      this._toastService.toast('success', data['result']);
      this.dialogRef.close();

    }, async err => {
      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.joinGroup();
      }
      this._toastService.toast('error', err.error['result']['GroupId'] || err.error['result']);
    });
  }

}
