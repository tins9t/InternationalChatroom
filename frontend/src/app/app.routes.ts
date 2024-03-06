import { Routes } from '@angular/router';
import { RoomComponent } from '../room/room.component';
import { SigninComponent } from '../signin/signin.component';

export const routes: Routes = [
  {
    path: 'room/:roomId',
    component: RoomComponent
  },
  {
    path: '',
    component: SigninComponent
  }
];
