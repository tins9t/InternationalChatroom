import { Component } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {FormControl, ReactiveFormsModule} from "@angular/forms";
import {WebsocketService} from "../service/websocket.service";
import {ClientWantsToBroadcastToRoom} from "../models/clientWantsToBroadcastToRoom";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-room',
  standalone: true,
  imports: [
    ReactiveFormsModule, CommonModule
  ],
  templateUrl: './room.component.html',
  styleUrl: './room.component.css'
})
export class RoomComponent {
  constructor(private route: ActivatedRoute, private service: WebsocketService) {
  }

  title: string | null = null;
  messagesContent = new FormControl('')

  getRoomId(){
    return this.route.snapshot.params['roomId'];
  }

  getRoomName(){
    this.getRoomId();
  }

  getMessages(){
    console.log("Messages: "+this.service.messages)
    return this.service.messages;
  }

  clickSend() {
    this.service.sendDto(new ClientWantsToBroadcastToRoom({roomId: Number.parseInt(this.getRoomId()), message: this.messagesContent.value!}))
  }
}
