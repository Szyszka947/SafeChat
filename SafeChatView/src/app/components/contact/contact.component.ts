import { Component, Input, OnInit } from '@angular/core';
import { GroupDto } from 'src/app/models/groupDto';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  constructor() { }

  @Input()
  groupDto: GroupDto | undefined;

  ngOnInit(): void {
  }

}
