import { Component } from '@angular/core';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { PlayerData } from '../../services/player-data/PlayerData';
import { CurrentPlayersMessage } from '../../services/current-players/events/CurrentPlayersMessage';
import { MatListItemIcon, MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-active-players',
  imports: [MatListModule, MatCardModule],
  templateUrl: './active-players.component.html',
  styleUrl: './active-players.component.css'
})
export class ActivePlayersComponent 
{
  private currentPlayersService: ActivePlayersService;

  public constructor(currentPlayersService: ActivePlayersService)
  {
    this.currentPlayersService = currentPlayersService;

    this.CurrentPlayers = this.currentPlayersService.CurrentPlayers;

    this.currentPlayersService.Subscribe({next: this.OnCurrentPlayersMessageReceived})
  }

  public CurrentPlayers: PlayerData[];

  private OnCurrentPlayersMessageReceived = (_: CurrentPlayersMessage): void =>
  {
    this.CurrentPlayers = [...this.currentPlayersService.CurrentPlayers];
  }
}
