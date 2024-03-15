import {BaseDto} from "./baseDto";

export class ClientWantsToBroadcastToRoom extends BaseDto<ClientWantsToBroadcastToRoom> {
  roomId?: number;
  message?: string;
}
