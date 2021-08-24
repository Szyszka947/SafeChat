import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Origins } from '../data/origins';
import { User } from '../data/user';
import { UserDto } from '../models/userDto';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  private basicHttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private httpOptionsWithCredentials = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true
  };

  createAccount(body: any) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/user/register', body, this.basicHttpOptions);
  }

  signIn(body: any) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/user/login', body, this.httpOptionsWithCredentials);
  }

  async isUserAuthenticated(): Promise<boolean> {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/auth/user/authenticated', this.httpOptionsWithCredentials).toPromise() as Promise<boolean>;
  }

  async getAccessTokenByRefreshToken(): Promise<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      withCredentials: true
    };

    return this.http.get(Origins.SafeChatAPIOrigin + 'api/auth/refresh_token', this.httpOptionsWithCredentials).toPromise() as Promise<boolean>;
  }

  async getUserInfo(): Promise<User> {
    return this.http.get(Origins.SafeChatAPIOrigin + 'api/auth/user_info', this.httpOptionsWithCredentials).toPromise() as Promise<User>;
  }

  logOut() {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/user/logout', {}, this.httpOptionsWithCredentials);
  }

  async getUsersWhoNameStartsWith(startsWith: string) {
    return await this.http.get(Origins.SafeChatAPIOrigin + 'api/user/get?startsWith=' + startsWith, this.httpOptionsWithCredentials).toPromise() as Promise<Array<UserDto>>;
  }
}
