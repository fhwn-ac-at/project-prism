import { Component, inject } from '@angular/core';
import { GameRoundService } from '../../services/game-round/game-round.service';
import { MatCard } from '@angular/material/card';

@Component({
  selector: 'app-round-display',
  imports: [MatCard],
  templateUrl: './round-display.component.html',
  styleUrl: './round-display.component.css'
})
export class RoundDisplayComponent 
{
  public GameRoundService: GameRoundService = inject(GameRoundService);
}
