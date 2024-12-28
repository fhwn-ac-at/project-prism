import { Component, inject, input, Input, InputSignal, OnInit } from '@angular/core';
import { DrawableCanvasComponent } from '../../components/drawable-canvas/component/drawable-canvas.component';
import { TopBarComponent } from '../../components/top-bar/top-bar.component';
import { CountdownService } from '../../services/countdown/countdown.service';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { IsPlayerData, PlayerData } from '../../services/player-data/PlayerData';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { just } from '@sweet-monads/maybe';
import { CurrentPlayersService } from '../../services/current-players/current-players.service';
import { ChatMessagesService } from '../../services/chat-messages/chat-messages.service';
import { ChatComponent } from "../../components/chat/chat.component";


@Component({
  selector: 'app-game-page',
  imports: [DrawableCanvasComponent, TopBarComponent, ChatComponent],
  templateUrl: './game-page.component.html',
  styleUrl: './game-page.component.css',
  providers: 
  [
    {provide: CountdownService, useClass: CountdownService},
    {provide: PlayerDataService, useClass: PlayerDataService},
    {provide: HiddenWordService, useClass: HiddenWordService},
    {provide: CurrentPlayersService, useClass: CurrentPlayersService},
    {provide: ChatMessagesService, useClass: ChatMessagesService},
  ]
})
export class GamePageComponent implements OnInit
{
  private data: PlayerData | undefined;

  private playerData: PlayerDataService;
  private countdown: CountdownService;
  private currentPlayers: CurrentPlayersService;

  public constructor
  (
    playerData: PlayerDataService,
    countdown: CountdownService, 
    currentPlayers: CurrentPlayersService,
  )
  {
    this.playerData = playerData;
    this.countdown = countdown;
    this.currentPlayers = currentPlayers;
  }

  @Input() public set Data(dataString: string) 
  {
    let val = JSON.parse(dataString);

    if (!IsPlayerData(val)) 
    {
      throw new Error("Must be of type PlayerData");
    } 

    this.data = val;
  }

  public ngOnInit(): void 
  {
    if (this.data) 
    {
      this.currentPlayers.CurrentPlayerData.Push(this.data);
      this.playerData.PlayerData.next(just<PlayerData>(this.data));
    }

    this.countdown.StartTimer(10, 500);
  }
}

