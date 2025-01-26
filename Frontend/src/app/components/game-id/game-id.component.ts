import { Component, inject } from '@angular/core';
import { GameIdService } from '../../services/gameId/game-id.service';
import { MatCard, MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-game-id',
  imports: [MatCardModule, MatDividerModule],
  templateUrl: './game-id.component.html',
  styleUrl: './game-id.component.css'
})
export class GameIdComponent 
{
  public IdService: GameIdService = inject(GameIdService);
}
