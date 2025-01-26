import { inject, Injectable } from '@angular/core';
import { PlayerDataService } from '../player-data/player-data.service';
import { CountdownService } from '../countdown/countdown.service';
import { PlayerType } from '../player-data/PlayerType';
import { PlayerTypeService } from '../player-type/player-type.service';

@Injectable({
  providedIn: null
})
export class CanDrawService 
{
  private playerTypeService: PlayerTypeService = inject(PlayerTypeService);
  private countdownService: CountdownService = inject(CountdownService);

  public IsDrawer(): boolean
  {
    return this.playerTypeService.PlayerType.value == PlayerType.Drawer;
  }

  public CanUseCanvas(): boolean
  {
    return this.playerTypeService.PlayerType.value == PlayerType.Drawer && this.countdownService.IsRunning();
  }

  public IsCountdownRunning(): boolean
  {
    return this.countdownService.IsRunning();
  }
}
