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
import { ApiService } from '../../networking/api.service';
import { WebSocketSubject, webSocket } from 'rxjs/webSocket';
import { HttpClient, HttpParams } from '@angular/common/http';

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
  private http: HttpClient = inject(HttpClient);

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

  public OnApiButtonClicked($event: MouseEvent) 
  {
    let params = new HttpParams().set("lobbyId", "test12345");

    this.http.get
    (
      "http://localhost:5164/api/Lobby/connect", 
      {
        params: params, 
        responseType: 'text'
      }
    )
    .subscribe
    (
      {
        next: (val) => console.log(val),
        error: (err) => console.log(err)
      }
    );
  }

  public OnWSButtonClicked($event: MouseEvent) 
  {
    webSocket("ws://localhost:5164/ws/test1").subscribe(
    {
      next: (v) => console.log(v), 
      error: (v) => console.log(v),
      complete: () => console.log("complete!")
    })
  }
}