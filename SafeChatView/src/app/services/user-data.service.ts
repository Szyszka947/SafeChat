import { Injectable } from '@angular/core';
import { User } from '../data/user';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private _accountService: AccountService) { }

  private user: User | undefined;

  async loadUser() {
    this.user = await this._accountService.getUserInfo();
  }

  public getUser(): User | undefined {
    return this.user;
  }
}
