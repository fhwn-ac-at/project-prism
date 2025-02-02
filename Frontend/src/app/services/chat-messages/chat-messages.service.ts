import { Injectable } from '@angular/core';
import { ObservableArray } from '../../../lib/observable/ObservableArray/ObservableArray';
import { ChatMessage } from './ChatMessage';
import * as dto from "../../networking/dtos/shared/chatMessage"
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { filter, Observable, Subject } from 'rxjs';
import { PlayerDataService } from '../player-data/player-data.service';
import { isGuessClose } from '../../networking/dtos/game/game-flow/guessClose.guard';
import { GuessClose } from '../../networking/dtos/game/game-flow/guessClose';

@Injectable({
  providedIn: null
})
export class ChatMessagesService 
{
  private gameApi: GameApiService;
  private playerDataService: PlayerDataService;

  private chatMessages: ChatMessage[];
  private chatMessageSubject: Subject<ChatMessage>;

  public constructor(gameApi: GameApiService, playerData: PlayerDataService) 
  {
    this.gameApi = gameApi;
    this.playerDataService = playerData;

    this.chatMessages = [];
    this.chatMessageSubject = new Subject<ChatMessage>();

    this.gameApi.ObserveChatMessageEvent()
    .subscribe(this.OnChatMessage);

    this.gameApi.ObserveGameFlowEvent()
    .pipe(filter(isGuessClose))
    .subscribe(this.OnGuessClose)
  }

  public get ChatMessages() : ChatMessage[]
  {
    return [...this.chatMessages];
  }

  public ObserveChatMessages(): Observable<ChatMessage>
  {
    return this.chatMessageSubject.asObservable();
  }

  private OnChatMessage = (val: dto.ChatMessage): void =>
  {
    this.AddChatMessage({Username: val.body.user.name, Message: val.body.text, Color: "Black"});
  }

  private OnGuessClose = (val: GuessClose): void =>
  {
    this.AddChatMessage({Username: "HINT", Color: "green", Message: "Your guess was off by " + val.body.distance })
  }

  public async AddAndSendChatMessage(message: string): Promise<void>
  {
    if (this.playerDataService.PlayerData.value == undefined) return Promise.reject(new Error("No player data!"));

    await this.gameApi.SendChatMessage(message);

    this.AddChatMessage({Username: this.playerDataService.PlayerData.value.Username, Message: message, Color: "Black"});
  }

  private AddChatMessage(message: ChatMessage): void
  {
    this.chatMessages.push(message);

    this.chatMessageSubject.next(message);
  }
}
