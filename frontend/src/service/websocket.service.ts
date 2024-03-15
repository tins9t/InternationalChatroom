import { Injectable } from '@angular/core';
import { BaseDto, ServerBroadcastsMessageWithUsernameDto } from '../models/baseDto';
import {Message, Translation} from '../models/entities';
import {firstValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  private messageQueue: Array<BaseDto<any>> = [];

  ws: WebSocket = new WebSocket("ws://localhost:8181")
  public messages: Message[] = [];
  languageCode: string = '';

  constructor(private http: HttpClient) {
    this.handleEvent();
  }

  setLanguageCode(code: string) {
    this.languageCode = code;
  }

  getLanguageCode(): string {
    return this.languageCode;
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

  async translateText(text: string): Promise<string> {
    try {
      const requestBody = { Text: text, To: this.getLanguageCode() };
      const response = await this.http.post('http://localhost:5082/api/translate', requestBody, { responseType: 'text' }).toPromise();

      // Log the response content
      console.log('Response:', response);

      return response!.trim(); // Trim any whitespace
    } catch (error) {
      console.error('Translation Error:', error);
      throw error;
    }
  }


  ServerBroadcastsMessageWithUsername(dto: ServerBroadcastsMessageWithUsernameDto) {
    if (dto.message !== undefined) {
      // Translate the message text
      this.translateText(dto.message.message!).then((translation) => {
        // Update the message text with the translated text
        dto.message!.text = translation;

        // Push the updated message into the messages array
        this.messages.push(dto.message!);
      }).catch((error) => {
        // Handle translation error if needed
        console.error("Translation Error:", error);
      });
    }
  }
}
