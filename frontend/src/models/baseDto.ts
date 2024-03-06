import { Message } from "./entities";

export class BaseDto<T>
{
  eventType: string;

  constructor(init?: Partial<T>)
  {
    this.eventType = this.constructor.name;
    Object.assign(this, init)
  }
}

export class ServerBroadcastsMessageWithUsernameDto extends BaseDto<ServerBroadcastsMessageWithUsernameDto>{
  message?: Message;
}