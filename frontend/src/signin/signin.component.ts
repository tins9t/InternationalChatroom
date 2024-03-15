import { Component } from '@angular/core';
import {CommonModule} from "@angular/common";
import {LanguageInfo, Theme} from '../models/entities';
import {WebsocketService} from "../service/websocket.service";
import {FormControl, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-signin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, FormsModule],
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css'
})
export class SigninComponent {
  buttons: Theme[] = [
    { roomId: 1, name: '🐶Animals🐶' },
    { roomId: 2, name: '⚽️Sport⚽️' },
    { roomId: 3, name: '🎮Gaming🎮' },
    { roomId: 4, name: '🥪Food🥪' },
    { roomId: 5, name: '🎥Movies🎥' },
    { roomId: 6, name: '📚Books📚' }
  ];
  username = new FormControl('')
  selectedChatroomId: number | null = null;
  selectedLanguage: string | null = null;
  languages: { [key: string]: LanguageInfo } = {};
  warning: string | null = null;

  constructor(private service: WebsocketService, private router: Router, private http : HttpClient) {
    this.getLanguages();
  }

  selectChatroom(roomId: number | undefined): void {
    this.selectedChatroomId = roomId ?? null;
    console.log("Chatroom ID: "+this.selectedChatroomId)
  }

  clickStart() {
    if(this.selectedChatroomId!=null && this.selectedLanguage!=null && this.username!=null){
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
    else{
      this.warning = "Please select username, language and room!"
    }
  }

  async getLanguages(){
    try {
      const call = this.http.get<{ [key: string]: LanguageInfo }>("http://localhost:5082/api/languages");
      this.languages = await firstValueFrom<{ [key: string]: LanguageInfo }>(call);
    } catch (error) {
      console.error('Error fetching languages: ', error);
    }
  }

  selectLanguage() {
    console.log("Selected language key: " + this.selectedLanguage);
  }
}
