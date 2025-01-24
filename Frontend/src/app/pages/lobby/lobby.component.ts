import { Component, inject } from '@angular/core';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { PlayerType } from '../../services/player-data/PlayerType';
import { TopBarComponent } from "../../components/top-bar/top-bar.component";
import { ChatComponent } from "../../components/chat/chat.component";
import { DrawableCanvasComponent } from "../../components/drawable-canvas/component/drawable-canvas.component";
import { LobbyOptionsComponent } from "../../components/lobby-options/lobby-options.component";
import { LobbyOptionsService } from '../../services/lobby-options/lobby-options.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-lobby',
  imports: [ActivePlayersComponent, TopBarComponent, ChatComponent, LobbyOptionsComponent],
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.css',
  providers: 
  [
    {provide: LobbyOptionsService, useClass: LobbyOptionsService}
  ]
})
export class LobbyComponent 
{  
  private lobbyService: LobbyOptionsService;
  router: Router;

  public constructor(lobby: LobbyOptionsService, router: Router)
  {
    this.lobbyService = lobby;
    this.router = router;
    
    this.lobbyService.GameStarted.subscribe((() => 
    {
      this.router.navigate(["/game"]);
    }));
  }
}
