export class Message {
  username?: string;
  message?: string;
}

export class Button {
  roomId?: number;
  name?: string;
}

export class LanguageRootObject {
  translation?: { [key: string]: LanguageInfo };
}

export class LanguageInfo {
  name?: string;
  nativeName?: string;
  dir?: string;
}
