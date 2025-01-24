import { Injectable } from '@angular/core';
import { ObservableArray } from '../../../lib/observable/ObservableArray/ObservableArray';
import { ChatMessage } from './ChatMessage';
import * as dto from "../../networking/dtos/shared/chatMessage"
import { GameApiService } from '../gameApi/game-api.service';

@Injectable({
  providedIn: null
})
export class ChatMessagesService 
{
  private gameApi: GameApiService;

  private chatMessages: ObservableArray<ChatMessage>;

  constructor(gameApi: GameApiService) 
  {
    this.gameApi = gameApi;

    this.chatMessages = new ObservableArray<ChatMessage>([]);

    this.gameApi.ObserveChatMessageEvent().subscribe({next: (val: dto.ChatMessage) => 
    {
      this.chatMessages.Push({Username: val.body.user.name, Message: val.body.text, Color: "Black"});
    }})
  }

  public get ChatMessages(): ObservableArray<ChatMessage>
  {
    return this.chatMessages;
  }
}
