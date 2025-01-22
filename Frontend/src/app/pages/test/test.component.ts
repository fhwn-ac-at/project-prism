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
  private apiService: ApiService = inject(ApiService);
  private signalRService: SignalRService = inject(SignalRService);

  public OnGamePageClicked(_: MouseEvent) 
  {
    this.playerDataService.PlayerData.next
    (
      just({ Username: "TestUser", Role: PlayerType.Drawer, Score: 0})
    );

    this.countdownService.StartTimer(50, 1000);
    this.roundsService.Initialize(3n);

    this.pickWordService.LetUserPickWord(["a", "b", "c", "d"]);

    this.pickWordService.SubscribeWordPicked({next: (ev) => {console.log(ev)}});

    this.hiddenWordService.SetWord("Hyponysenadresom");

    for(let i = 0; i < 50; i++)
    {
      this.activePlayersService.Add({Username: "TestName" + i, Role:Math.round( Math.random()), Score: Math.round(Math.random() * 100)});
    }
  }

  public OnLobbyPageClicked(_: MouseEvent) 
  {
    this.playerDataService.PlayerData.next
    (
      just({ Username: "TestUser", Role: PlayerType.Drawer, Score: 0})
    );
  }

  private id: string | undefined = undefined;

  public OnConnectClicked($event: MouseEvent) 
  {
    this.apiService.ConnectToLobby("test").subscribe({next: (v) => 
    {
      console.log("received connect response!:" + v);
      this.id = v;
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
    this.signalRService.Initialize(this.id!);

    this.signalRService.SignalRHub!.on("Backend", (m) => {
      console.log("Received data on Backend:");
      console.log(m);
    });

    this.signalRService.SignalRHub!.on("Fronted", (m) => {
      console.log("Received data on Fronted:");
      console.log(m);
    });
    
    await this.signalRService.SignalRHub!.start().then(() => console.log("Started!"), (v) => console.log(v));
  }

  public async OnSendClicked($event: MouseEvent)
  {
    this.signalRService.SignalRHub!.send("Backend", "testMessage");
  }
}