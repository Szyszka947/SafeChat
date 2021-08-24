import { Component, Input, OnInit } from '@angular/core';
import { MessageDto } from 'src/app/models/messageDto';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  constructor() { }

  @Input()
  messageDto: MessageDto | undefined;

  @Input()
  left: boolean | undefined;

  ngOnInit(): void {
  }

  activateFullScreenMode(img: HTMLImageElement) {
    img.requestFullscreen();
  }

}
