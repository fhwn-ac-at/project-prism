import { Component, Input } from '@angular/core';
import { DrawableCanvasComponent } from '../../components/drawable-canvas/component/drawable-canvas.component';
import { TopBarComponent } from '../../components/top-bar/top-bar.component';
import { CountdownService } from '../../services/countdown/countdown.service';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { IsPlayerData } from '../../services/player-data/PlayerData';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { just } from '@sweet-monads/maybe';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ChatMessagesService } from '../../services/chat-messages/chat-messages.service';
import { ChatComponent } from "../../components/chat/chat.component";
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";

@Component({
  selector: 'app-game-page',
  imports: [DrawableCanvasComponent, TopBarComponent, ChatComponent, ActivePlayersComponent],
  templateUrl: './game-page.component.html',
  styleUrl: './game-page.component.css',
  providers: 
  [
    {provide: CountdownService, useClass: CountdownService},
    {provide: HiddenWordService, useClass: HiddenWordService},
    {provide: ActivePlayersService, useClass: ActivePlayersService},
    {provide: ChatMessagesService, useClass: ChatMessagesService},
  ]
})
export class GamePageComponent
{
  // services
  private playerData: PlayerDataService;
  private countdown: CountdownService;
  private currentPlayers: ActivePlayersService;
  private hiddenWordService: HiddenWordService;

  public constructor
  (
    playerData: PlayerDataService,
    countdown: CountdownService, 
    currentPlayers: ActivePlayersService,
    hiddenWordService: HiddenWordService,
  )
  {
    this.playerData = playerData;
    this.countdown = countdown;
    this.currentPlayers = currentPlayers;
    this.hiddenWordService = hiddenWordService;
  }
}

