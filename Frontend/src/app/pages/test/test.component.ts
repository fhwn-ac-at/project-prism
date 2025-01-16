import { Component } from '@angular/core';
import { PlayerType } from '../../services/player-data/PlayerType';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { just } from '@sweet-monads/maybe';

@Component({
  selector: 'app-test',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent 
{
  private playerDataService: PlayerDataService;

  public constructor(playerData: PlayerDataService)
  {
    this.playerDataService = playerData;
  }

  public OnGamePageClicked(_: MouseEvent) 
  {
    this.playerDataService.PlayerData.next
    (
      just({ Username: "TestUser", Role: PlayerType.Guesser})
    );
  }
}
