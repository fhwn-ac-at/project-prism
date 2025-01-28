import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { PlayerData } from '../../services/player-data/PlayerData';
import { MatButton } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-show-scores',
  imports: [MatDialogModule, MatButton, MatListModule],
  templateUrl: './show-scores.component.html',
  styleUrl: './show-scores.component.css'
})
export class ShowScoresComponent 
{
  private router: Router = inject(Router);

  public constructor()
  {
    const data = inject(MAT_DIALOG_DATA);

    this.Word = data.Word;

    let pd: PlayerData[] = data.PlayerData;

    this.PlayerData = pd.sort((a, b) => 
    {
      if (a.Score == b.Score)
      {
        return 0;
      }
      else if(a.Score < b.Score)
      {
        return 1;
      }
      else
      {
        return -1;
      }
    });

    this.GameEnded = data.IsEnded;
  }

  public Word: string;
  public GameEnded: boolean;
  public PlayerData: PlayerData[];
}
