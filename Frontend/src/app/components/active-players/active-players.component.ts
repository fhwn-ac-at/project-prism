import { Component, inject, OnDestroy } from '@angular/core';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { PlayerData } from '../../services/player-data/PlayerData';
import { CurrentPlayersMessage } from '../../services/current-players/events/CurrentPlayersMessage';
import { MatListItemIcon, MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { NgClass } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-active-players',
  imports: [MatListModule, MatCardModule, NgClass],
  templateUrl: './active-players.component.html',
  styleUrl: './active-players.component.css'
})
export class ActivePlayersComponent implements OnDestroy
{
  private currentPlayersService: ActivePlayersService;

  private sub1: Subscription;

  public constructor(currentPlayersService: ActivePlayersService)
  {
    this.currentPlayersService = currentPlayersService;

    this.CurrentPlayers = this.currentPlayersService.CurrentPlayers;

    this.sub1 = this.currentPlayersService.ObserveCurrentPlayersEvent().subscribe({next: this.OnCurrentPlayersMessageReceived});
  }

  public PlayerDataService: PlayerDataService = inject(PlayerDataService);

  public CurrentPlayers: PlayerData[];

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();
  }

  private OnCurrentPlayersMessageReceived = (_: CurrentPlayersMessage): void =>
  {
    this.CurrentPlayers = [...this.currentPlayersService.CurrentPlayers];
  }
}
