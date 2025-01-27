import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';
import { PlayerType } from '../player-data/PlayerType';
import { GameApiService } from '../../networking/services/game-api/game-api.service';
import { isSetDrawer } from '../../networking/dtos/game/game-flow/setDrawer.guard';
import { isSetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer.guard';
import { SetDrawer } from '../../networking/dtos/game/game-flow/setDrawer';
import { SetNotDrawer } from '../../networking/dtos/game/game-flow/setNotDrawer';

@Injectable({
  providedIn: null
})
export class PlayerTypeService 
{
  private gameApiService: GameApiService = inject(GameApiService);

  public constructor()
  {
        this.gameApiService.ObserveGameFlowEvent()
        .pipe(filter((val) => isSetDrawer(val) || isSetNotDrawer(val)))
        .subscribe(this.OnPlayerType)
  }

  public PlayerType: BehaviorSubject<PlayerType> = new BehaviorSubject<PlayerType>(PlayerType.NotSet);

  private OnPlayerType = (value: SetDrawer | SetNotDrawer) =>
  {
    const role = isSetDrawer(value) ? PlayerType.Drawer : PlayerType.Guesser;

    this.PlayerType.next(role);
  }
}
