import { AfterViewInit, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { map } from 'rxjs/operators';
import { GroupDto } from 'src/app/models/groupDto';
import { GroupService } from 'src/app/services/group.service';
import { Origins } from 'src/app/data/origins';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorsHandlerService } from 'src/app/services/errors-handler.service';
import { MessageDto } from 'src/app/models/messageDto';
import { UserDataService } from 'src/app/services/user-data.service';
import { FormControl, FormGroup } from '@angular/forms';
import { MessageService } from 'src/app/services/message.service';
import { ToastService } from 'src/app/services/toast.service';
import { MatDialog } from '@angular/material/dialog';
import { CreateChatDialogComponent } from 'src/app/dialogs/create-chat-dialog/create-chat-dialog.component';
import { JoinChatDialogComponent } from 'src/app/dialogs/join-chat-dialog/join-chat-dialog.component';
import { AccountService } from 'src/app/services/account.service';
import { Router } from '@angular/router';
import { LeaveChatDialogComponent } from 'src/app/dialogs/leave-chat-dialog/leave-chat-dialog.component';
import { PublicGroupDto } from 'src/app/models/publicGroupDto';
import { InviteUserDialogComponent } from 'src/app/dialogs/invite-user-dialog/invite-user-dialog.component';
import { KickUserDialogComponent } from 'src/app/dialogs/kick-user-dialog/kick-user-dialog.component';
import { BanUserDialogComponent } from 'src/app/dialogs/ban-user-dialog/ban-user-dialog.component';
import { MyInvitationsDialogComponent } from 'src/app/dialogs/my-invitations-dialog/my-invitations-dialog.component';
import { InvitationDto } from 'src/app/models/invitationDto';
import { HttpTransportType, HubConnectionBuilder, IHttpConnectionOptions, LogLevel } from '@microsoft/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor(private _groupService: GroupService, private _errorsHandlerService: ErrorsHandlerService, private _userDataService: UserDataService
    , private _messageService: MessageService, private renderer: Renderer2, private _toastService: ToastService, private dialog: MatDialog,
    private _accountService: AccountService, private router: Router) { }

  ngAfterViewInit(): void {
    this.renderer.listen(this.messagesList.nativeElement, 'scroll', () => {
      if (this.messagesList.nativeElement.scrollTop == 0 && this.messagesList.nativeElement.childElementCount < this.allGroupMessages.length) {
        this.loadGroupMessages(this.allGroupMessages, this.groupMessages.length + 25);
        this.scrollMessagesToLastScrollTop();
      }
    });
  }

  //configure signalr
  connection = new HubConnectionBuilder()
    .configureLogging(LogLevel.Information)
    .withUrl(Origins.SafeChatAPIOrigin + 'chatHub')
    .withAutomaticReconnect()
    .build();

  groupList: Array<GroupDto> = new Array<GroupDto>();
  groupMessages: Array<MessageDto> = new Array<MessageDto>();
  myName: string | undefined;
  images: Array<string> = new Array<string>();
  allGroupMessages: Array<MessageDto> = new Array<MessageDto>();
  groupSelected: boolean = false;
  currentGroupName: string = "";
  currentGroupId: number = -1;
  latestLast: MessageDto | undefined;
  groupInvitations: Array<InvitationDto> | undefined;

  @ViewChild('messagesList')
  messagesList!: ElementRef;

  @ViewChild('file')
  file!: ElementRef;

  @ViewChild('sendMessageBtn')
  sendMessageBtn!: ElementRef;

  @ViewChild('formData')
  formData!: ElementRef;

  @ViewChild('groupULList')
  groupULList!: ElementRef;

  async deleteGroup(groupId: number) {
    const group: HTMLLIElement = (this.groupULList.nativeElement as HTMLUListElement).querySelector('#group-' + groupId) as HTMLLIElement;

    this.renderer.removeChild(this.groupULList.nativeElement as HTMLUListElement, group);
    this.groupList.splice(parseInt(group.id.replace('group-', '')), 1);

    this.groupMessages = new Array<MessageDto>();
    this.groupSelected = false;
    this.currentGroupId = -1;
    this.currentGroupName = "";
  }

  async ngOnInit(): Promise<void> {
    this.myName = this._userDataService.getUser()?.UserName;
    this.groupInvitations = await this._groupService.getGroupInvitations();

    this._groupService.getUserGroups().pipe(map(data => data as any)).subscribe(data => {
      this.groupList = data['result'] as Array<GroupDto>;
    });

    this.connection.start();

    //signalr methods

    //Received Message
    this.connection.on('ReceivedMessage', async (groupId: number, content: string, imageUrls: Array<string>, dateTime: string, sender: string, type: number) => {
      const list = this.messagesList.nativeElement as HTMLUListElement;
      const messageDto: MessageDto = { groupId, content, imageUrls, dateTime, senderName: sender, messageType: type };
      if (list.scrollHeight - list.scrollTop === list.clientHeight) {
        this.groupMessages.push(messageDto);
        setTimeout(() => {
          this.scrollMessagesDown();
        }, 50);
      }
      else this.groupMessages.push(messageDto);
    });

    //Received Group Join
    this.connection.on('ReceivedGroupJoin', async (groupId: number, groupName: string, isGroupPublic: boolean) => {
      let newGroup = new GroupDto();
      newGroup.id = groupId;
      newGroup.groupName = groupName;
      newGroup.isGroupPublic = isGroupPublic;

      this.groupList.push(newGroup);

      await this.showSelectedGroupMessages(groupId, groupName);
    });

    //Received Group Leave
    this.connection.on('ReceivedGroupLeave', async (groupId: number) => {
      await this.deleteGroup(groupId);
    });

    //Received Member To Kick
    this.connection.on('ReceivedMemberToKick', async (groupId: number, info: string) => {
      await this.deleteGroup(groupId);
      this._toastService.toast('warning', info);
    });

    //Received Member To Ban
    this.connection.on('ReceivedMemberToBan', async (groupId: number, info: string) => {
      await this.deleteGroup(groupId);
      this._toastService.toast('warning', info);
    });

    //Received Invitation Action
    this.connection.on('ReceivedInvitationAction', async (groupId: number) => {
      for (let i = 0; i <= (this.groupInvitations as Array<InvitationDto>).length; i++) {
        if ((this.groupInvitations as Array<InvitationDto>)[i].groupId == groupId) {
          this.groupInvitations?.splice(i, 1);
        }
      }
    });

    //Received New Invitation
    this.connection.on('ReceivedNewInvitation', async (groupId: number, groupName: string) => {
      let invitation = new InvitationDto();
      invitation.groupId = groupId;
      invitation.groupName = groupName;

      this.groupInvitations?.push(invitation);
    });
  }

  sendMessageForm: FormGroup = new FormGroup({
    Message: new FormControl('')
  });

  scrollMessagesDown(): void {
    return this.messagesList.nativeElement.scrollTop = this.messagesList.nativeElement.scrollHeight;
  }

  scrollMessagesToLastScrollTop(): void {
    const list = (this.messagesList.nativeElement as HTMLUListElement);
    const childScrollHeight = list.children.item(this.groupMessages.length - 25 - 1)?.scrollHeight as number;

    list.scrollTop = childScrollHeight;
  }

  loadGroupMessages(data: Array<MessageDto>, numberOfMessagesToLoad: number) {
    this.groupMessages = data.slice(0, numberOfMessagesToLoad).reverse();
  }

  async showSelectedGroupMessages(groupId: number, groupName: string): Promise<void> {
    this._groupService.signalRJoinGroup(this.connection.connectionId, groupId).pipe(map(data => data as Array<MessageDto>)).subscribe(data => {
      this.groupSelected = true;
      if (sessionStorage.getItem('currentGroup') != groupId.toString()) {
        this.groupMessages = new Array<MessageDto>();
      }

      this.allGroupMessages = data;
      sessionStorage.setItem('currentGroup', groupId.toString());
      this.currentGroupName = groupName;
      this.currentGroupId = groupId;

      this.loadGroupMessages(data, 25);

      setTimeout(() => {
        this.scrollMessagesDown();
      }, 200);

    }, async (err: HttpErrorResponse) => {
      if (err.status == 400) {
        this._toastService.toast('warning', err.error['result']['ConnectionId'] || err.error['result']);
      }

      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return await this.showSelectedGroupMessages(groupId, groupName);
      }
    });
  }

  sendMessageByEnterPress(e: any) {
    if (e.keyCode == 13) { this.sendMessage(this.formData.nativeElement); e.preventDefault(); };
  }

  async sendMessage(form: HTMLFormElement) {
    this.sendMessageForm.setErrors(null);

    let formData = new FormData(form);
    formData.append('GroupId', sessionStorage.getItem('currentGroup') as string);

    this._messageService.sendMessage(formData).subscribe(data => {
      this.sendMessageForm.controls['Message'].setValue('');
      this.images = new Array<string>();
      this.scrollMessagesDown();
      this.file.nativeElement.value = '';

    }, async err => {
      if (err.status == 400) {
        const specificErrorsExists = err.error['result']['GroupId'] != undefined || err.error['result']['IsMessageValid'] != undefined;

        this._toastService.toast('error', specificErrorsExists ? err.error['result']['GroupId'][0] || err.error['result']['IsMessageValid'][0] : err.error['result']);
      }

      if (err.status == 401) {
        await this._errorsHandlerService.handle401StatusCode();
        return this.sendMessage(form);
      }
    });
  }

  fileSelected() {
    const files = this.file.nativeElement.files;
    for (let index = 0; index < files.length; index++) {
      const file = files[index];
      const reader = new FileReader();
      reader.onload = (event) => {
        this.images.push(event.target?.result as string);
      };
      reader.readAsDataURL(file);
    }
  }

  removeSelectedFile(img: HTMLImageElement) {
    const index = parseInt(img.id);

    this.images.splice(index, 1);

    const dt = new DataTransfer();
    const input = this.file.nativeElement;
    const { files } = input;
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      if (index !== i) dt.items.add(file);
      input.files = dt.files;
    }
  }

  showCreateChatUI() {
    this.dialog.open(CreateChatDialogComponent);
  }

  showJoinChatUI() {
    this.dialog.open(JoinChatDialogComponent);
  }

  logOut() {
    this._accountService.logOut().subscribe();
    this.router.navigate(['user/login']);
  }

  showLeaveChatUI() {
    const groupDto = new PublicGroupDto();
    groupDto.id = this.currentGroupId;
    groupDto.groupName = this.currentGroupName;

    this.dialog.open(LeaveChatDialogComponent, {
      data: groupDto
    });
  }

  showInviteUserUI() {
    this.dialog.open(InviteUserDialogComponent, {
      data: this.currentGroupId
    });
  }

  showKickUserUI() {
    this.dialog.open(KickUserDialogComponent, {
      data: this.currentGroupId
    });
  }

  showBanUserUI() {
    this.dialog.open(BanUserDialogComponent, {
      data: this.currentGroupId
    });
  }

  showMyInvitationsUI() {
    this.dialog.open(MyInvitationsDialogComponent, {
      data: this.groupInvitations,
      maxHeight: '400px'
    });
  }
}