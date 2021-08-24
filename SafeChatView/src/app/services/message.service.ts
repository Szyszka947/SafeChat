import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Origins } from '../data/origins';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private http: HttpClient) { }

  public sendMessage(formData: FormData) {
    return this.http.post(Origins.SafeChatAPIOrigin + 'api/message/send', formData, { withCredentials: true });
  }
}