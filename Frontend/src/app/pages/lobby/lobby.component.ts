import { Component, inject } from '@angular/core';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { PlayerType } from '../../services/player-data/PlayerType';
import { TopBarComponent } from "../../components/top-bar/top-bar.component";
import { ChatComponent } from "../../components/chat/chat.component";
import { DrawableCanvasComponent } from "../../components/drawable-canvas/component/drawable-canvas.component";
import { LobbyOptionsComponent } from "../../components/lobby-options/lobby-options.component";
import { LobbyService } from '../../services/lobby/lobby.service';
import { Router } from '@angular/router';
import { GameIdComponent } from "../../components/game-id/game-id.component";

@Component({
  selector: 'app-lobby',
  imports: [ActivePlayersComponent, TopBarComponent, ChatComponent, LobbyOptionsComponent],
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.css',
  providers: 
  [
    {provide: LobbyService, useClass: LobbyService}
  ]
})
export class LobbyComponent 
{  
  private lobbyService: LobbyService;
  private router: Router;

  public constructor(lobby: LobbyService, router: Router)
  {
    this.lobbyService = lobby;
    this.router = router;
    
    this.lobbyService.GameStarted.subscribe((() => 
    {
      this.router.navigate(["/game"]);
    }));
  }
}
