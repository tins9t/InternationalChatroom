import { Injectable } from '@angular/core';
import { BaseDto, ServerBroadcastsMessageWithUsernameDto } from '../models/baseDto';
import { Message } from '../models/entities';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  private messageQueue: Array<BaseDto<any>> = [];

  ws: WebSocket = new WebSocket("ws://localhost:8181")
  public messages: Message[] = [];

  constructor() {
    this.handleEvent();
  }

  sendDto(dto: BaseDto<any>) {
    console.log("Sending: " + JSON.stringify(dto));
    if (this.ws.readyState === WebSocket.OPEN) {
      this.ws.send(JSON.stringify(dto));
    } else {
      this.messageQueue.push(dto);
    }
  }

  handleEvent() {
    this.ws.onmessage = (event) => {
      const data = JSON.parse(event.data) as BaseDto<any>;
      console.log("Received: " + JSON.stringify(data));
      //@ts-ignore
      this[data.eventType].call(this, data);
    }
  }

  ServerBroadcastsMessageWithUsername(dto: ServerBroadcastsMessageWithUsernameDto) {
    if (dto.message !== undefined) {
      this.messages.push(dto.message);
    }
  }
}
