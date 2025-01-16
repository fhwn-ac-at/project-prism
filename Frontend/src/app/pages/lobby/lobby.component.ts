import { Component } from '@angular/core';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { PlayerType } from '../../services/player-data/PlayerType';

@Component({
  selector: 'app-lobby',
  imports: [ActivePlayersComponent],
  templateUrl: './lobby.component.html',
  styleUrl: './lobby.component.css',
  providers: [{provide: ActivePlayersService, useClass: ActivePlayersService}]
})
export class LobbyComponent 
{
  private currentPlayersService: ActivePlayersService;

  public constructor(currentPlayersService: ActivePlayersService, playerData: PlayerDataService)
  {
    this.currentPlayersService = currentPlayersService;
    this.PlayerDataService = playerData;
  }

  public PlayerType = PlayerType;

  public PlayerDataService: PlayerDataService;
}
