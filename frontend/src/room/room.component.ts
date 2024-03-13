import { Component } from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-room',
  standalone: true,
  imports: [],
  templateUrl: './room.component.html',
  styleUrl: './room.component.css'
})
export class RoomComponent {
  constructor(private route: ActivatedRoute) {
  }

  getRoomId(){
    return this.route.snapshot.params['roomId'];
  }
}
