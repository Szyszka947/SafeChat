import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ErrorsHandlerService } from '../services/errors-handler.service';
import { UserDataService } from '../services/user-data.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private _accountService: AccountService, private _userDataService: UserDataService, private _errorsHandlerService: ErrorsHandlerService) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {

    var isUserAuthenticated = await this._accountService.isUserAuthenticated();

    if (isUserAuthenticated) {
      await this._userDataService.loadUser();
      return true;
    }
    else {
      return await this._errorsHandlerService.handle401StatusCode();
    }
  }
}
