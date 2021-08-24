import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { UserDataService } from 'src/app/services/user-data.service';

@Component({
  selector: 'app-refresh-token',
  templateUrl: './refresh-token.component.html',
  styleUrls: ['./refresh-token.component.css']
})
export class RefreshTokenComponent implements OnInit {

  constructor(private _accountService: AccountService, private location: Location, private router: Router, private _userDataService: UserDataService) { }

  async ngOnInit(): Promise<void> {

  }
}
