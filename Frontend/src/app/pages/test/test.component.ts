import { Component, inject } from '@angular/core';
import { PlayerType } from '../../services/player-data/PlayerType';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { just } from '@sweet-monads/maybe';
import { CountdownService } from '../../services/countdown/countdown.service';
import { GameRoundService } from '../../services/game-round/game-round.service';
import { PickWordService } from '../../services/pick-word/pick-word.service';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ApiService } from '../../networking/services/api/api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { KeycloakEventType, KeycloakService } from 'keycloak-angular';
import { SignalRService } from '../../networking/services/signal-r/signal-r.service';
import { LobbyApiService } from '../../services/lobby-api/lobby-api.service';
import { GameApiService } from '../../services/gameApi/game-api.service';

@Component({
  selector: 'app-test',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent 
{
  private playerDataService: PlayerDataService = inject(PlayerDataService);
  private countdownService: CountdownService = inject(CountdownService);
  private roundsService: GameRoundService = inject(GameRoundService);
  private pickWordService: PickWordService = inject(PickWordService);
  private hiddenWordService: HiddenWordService = inject(HiddenWordService);
  private activePlayersService: ActivePlayersService = inject(ActivePlayersService);
  private apiService: LobbyApiService = inject(LobbyApiService);
  private gameService: GameApiService = inject(GameApiService);

  public OnGamePageClicked(_: MouseEvent) 
  {
    this.playerDataService.PlayerData.next
    (
      just({ Username: "TestUser", Role: PlayerType.Drawer, Score: 0, Id: "testId"})
    );

    this.countdownService.StartTimer(50, 1000);
    this.roundsService.Initialize(3n);

    this.pickWordService.LetUserPickWord(["a", "b", "c", "d"]);

    this.pickWordService.SubscribeWordPicked({next: (ev) => {console.log(ev)}});

    this.hiddenWordService.SetWord("Hyponysenadresom");

    for(let i = 0; i < 50; i++)
    {
      this.activePlayersService.Add(
        {Username: "TestName" + i, Role:Math.round( Math.random()), Score: Math.round(Math.random() * 100), Id: "testId"}
      );
    }
  }

  public OnLobbyPageClicked(_: MouseEvent) 
  {
    this.playerDataService.PlayerData.next
    (
      just({ Username: "TestUser", Role: PlayerType.Drawer, Score: 0, Id: "testId"})
    );
  }

  // basic api test

  private id: string | undefined = undefined;

  public OnConnectClicked($event: MouseEvent) 
  {
    this.apiService.ConnectToLobby("test").subscribe({next: (v) => 
    {
      console.log("received connect response!:" + v);
      this.id = v.id;
      this.playerDataService.PlayerData.next
    (
      just({ Username: v.name, Role: PlayerType.Drawer, Score: 0, Id: v.id})
    );
    }});
  }

  public OnStartClicked($event: MouseEvent) 
  {
    this.apiService.StartGame("test").subscribe({next: (v) => 
    {
      console.log("received start response!:" + v);
    }});
  }

  public async OnWSButtonClicked($event: MouseEvent) 
  {
    await this.gameService
      .Start(this.id!)
      .then(() => console.log("Started!"), (v) => console.log(v));
    
  }

  public async OnSendClicked($event: MouseEvent)
  {
    await this.gameService
      .SendChatMessage("Hello World!")
      .then(() => console.log("Sent!"), (v) => console.log(v));
  }
}