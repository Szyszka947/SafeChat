import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Origins } from '../data/origins';
import { GroupDto } from '../models/groupDto';
import { InvitationDto } from '../models/invitationDto';

@Injectable({
  providedIn: 'root'
})
export class GroupService {

  constructor(private http: HttpClient) { }

  private basicHttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private httpOptionsWithCredentials = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true
  };

  public getUserGroups() {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/user/groups', this.httpOptionsWithCredentials)
  }

  // its just join to signalr group for show group messages
  public signalRJoinGroup(connectionId: string | null, groupId: number) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/signalrgroup/join', { connectionId, groupId }, this.httpOptionsWithCredentials);
  }

  public createGroup(groupName: string, isGroupPublic: boolean) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/group/create', { groupName, isGroupPublic }, this.httpOptionsWithCredentials);
  }

  public getAllPublicGroups() {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/group/public/all', this.httpOptionsWithCredentials);
  }

  public joinGroup(groupId: number) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/group/join', { groupId }, this.httpOptionsWithCredentials);
  }

  public leaveGroup(groupId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
      body: {
        groupId
      }
    };
    return this.http.delete(Origins.SafeChatAPIOrigin + 'api/group/leave', httpOptions);
  }

  public inviteToGroup(groupId: number, invitedUserId: number) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/group/invite', { groupId, invitedUserId }, this.httpOptionsWithCredentials);
  }

  public getGroupMembers(groupId: number) {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/group/members?groupId=' + groupId, this.httpOptionsWithCredentials);
  }

  public kickUser(groupId: number, userToKickId: number, reason: string) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
      body: {
        groupId,
        userToKickId,
        reason
      }
    };
    return this.http.delete(Origins.SafeChatAPIOrigin + 'api/groupadmin/member/kick', httpOptions);
  }

  public banUser(groupId: number, userToBanId: number, reason: string, banForDays: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
      body: {
        groupId,
        userToBanId,
        reason,
        banForDays
      }
    };
    return this.http.delete(Origins.SafeChatAPIOrigin + 'api/groupadmin/member/ban', httpOptions);
  }

  public async getGroupInvitations(): Promise<Array<InvitationDto>> {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/groupinvitations/get', this.httpOptionsWithCredentials).toPromise() as Promise<Array<InvitationDto>>;
  }

  public acceptGroupInvitation(groupId: number) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/groupinvitations/accept', groupId, this.httpOptionsWithCredentials);
  }

  public discardGroupInvitation(groupId: number) {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      withCredentials: true,
      body: groupId
    };
    return this.http.delete(Origins.SafeChatAPIOrigin + 'api/groupinvitations/discard', httpOptions);
  }
}
