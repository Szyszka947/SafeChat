import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from './account.service';
import { UserDataService } from './user-data.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorsHandlerService {

  constructor(private _accountService: AccountService, private _userDataService: UserDataService, private router: Router) { }

  public async handle401StatusCode(): Promise<boolean> {
    window.history.pushState('get_access_token', document.title, 'api/auth/refresh_token');
    const isAuthenticated = await this._accountService.getAccessTokenByRefreshToken();

    window.history.pushState('', document.title, '');
    if (isAuthenticated == true) {
      await this._userDataService.loadUser();
      return true;
    }
    else {
      this.router.navigate(['user/login']);
      return false;
    }
  }
}
