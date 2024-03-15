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

  title: string | null = "🐶Animals🐶";
  messagesContent = new FormControl('')

  getRoomId(){
    return this.route.snapshot.params['roomId'];
  }

  getCurrentDateTime(): string {
    const currentDate = new Date();
    const date = currentDate.toLocaleDateString();
    const time = currentDate.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    return `${date} ${time}`;
  }

  getRoomName(){
    this.getRoomId();
  }

  getMessages(){
    return this.service.messages;
  }

  clickSend() {
    this.service.sendDto(new ClientWantsToBroadcastToRoom({roomId: Number.parseInt(this.getRoomId()), message: this.messagesContent.value!}))
    const messages = this.getMessages();
    messages.forEach((message) => {
      console.log("Message Text: ", message.text); // Print the text of each message
    });
    console.log("Language code: "+this.service.getLanguageCode())
    this.messagesContent.setValue('');
  }
}
