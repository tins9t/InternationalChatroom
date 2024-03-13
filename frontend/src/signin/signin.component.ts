import { Component } from '@angular/core';
import {CommonModule} from "@angular/common";
import {Button, LanguageRootObject} from '../models/entities';
import {WebsocketService} from "../service/websocket.service";
import {FormControl, ReactiveFormsModule} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css'
})
export class SigninComponent {
  buttons: Button[] = [
    { roomId: 1, name: '🐶Animals🐶' },
    { roomId: 2, name: '⚽️Sport⚽️' },
    { roomId: 3, name: '🎮Gaming🎮' },
    { roomId: 4, name: '🥪Food🥪' },
    { roomId: 5, name: '🎥Movies🎥' },
    { roomId: 6, name: '📚Books📚' }
  ];
  username = new FormControl('')
  selectedChatroomId: number | null = null;
  languages: LanguageRootObject[] = [];

  constructor(private service: WebsocketService, private router: Router, private http : HttpClient) {
  }

  selectChatroom(roomId: number | undefined): void {
    this.selectedChatroomId = roomId ?? null;
    console.log("Chatroom ID: "+this.selectedChatroomId)
  }

  clickStart() {
    if(this.selectedChatroomId!=null){
      var object = {
        eventType: "ClientWantsToSignIn",
        username: this.username.value
      }
      this.service.ws.send(JSON.stringify(object))
      var object2 = {
        eventType: "ClientWantsToEnterRoom",
        roomId: this.selectedChatroomId
      }
      this.service.ws.send(JSON.stringify(object2))
      this.router.navigate(['/room/' + this.selectedChatroomId], {replaceUrl: true})
    }
  }

  async getLanguages(){
    const call = this.http.get<LanguageRootObject[]>("http://localhost:5082/api/languages");
  }
}
