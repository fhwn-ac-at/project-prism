import { inject, Injectable } from '@angular/core';
import { PlayerDataService } from '../player-data/player-data.service';
import { CountdownService } from '../countdown/countdown.service';
import { PlayerType } from '../player-data/PlayerType';

@Injectable({
  providedIn: null
})
export class CanDrawService 
{
  private playerDataService: PlayerDataService = inject(PlayerDataService);
  private countdownService: CountdownService = inject(CountdownService);

  public IsDrawer(): boolean
  {
    if(this.playerDataService.PlayerData.value.isNone()) return false;

    return this.playerDataService.PlayerData.value.value.Role == PlayerType.Drawer;
  }

  public CanUseCanvas(): boolean
  {
      if (this.playerDataService.PlayerData.value.isNone()) return false;

      if (this.playerDataService.PlayerData.value.value.Role != PlayerType.Drawer || !this.countdownService.IsRunning()) 
      {
          return false;
      }

      return true;
  }

  public IsCountdownRunning(): boolean
  {
    return this.countdownService.IsRunning();
  }
}
