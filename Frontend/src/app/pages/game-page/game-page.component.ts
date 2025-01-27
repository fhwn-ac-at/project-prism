import { Component, inject } from '@angular/core';
import { DrawableCanvasComponent } from '../../components/drawable-canvas/component/drawable-canvas.component';
import { TopBarComponent } from '../../components/top-bar/top-bar.component';
import { ChatComponent } from "../../components/chat/chat.component";
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import { PickWordComponent } from '../../components/pick-word/pick-word.component';
import { PickWordService } from '../../services/pick-word/pick-word.service';
import { WordsToPickEvent } from '../../services/pick-word/events/WordsToPick';
import { ShowScoresService } from '../../services/show-scores/show-scores.service';
import { ShowScoresEvent } from '../../services/show-scores/events/ShowScoresEvent';
import { ShowScoresComponent } from '../../components/show-scores/show-scores.component';
import { timer } from 'rxjs';

@Component
(
  {
  selector: 'app-game-page',
  imports: [DrawableCanvasComponent, TopBarComponent, ChatComponent, ActivePlayersComponent],
  templateUrl: './game-page.component.html',
  styleUrl: './game-page.component.css',
  providers: []
  }
)
export class GamePageComponent
{
  private dialog: MatDialog = inject(MatDialog);
  private pickWordService: PickWordService = inject(PickWordService);
  private showScoresService: ShowScoresService = inject(ShowScoresService);

  public constructor()
  {
    this.pickWordService.ObserveWordsToPickEvent()
      .subscribe(this.OnWordsToPick);

    this.showScoresService.ObserveShowScoresEvent()
      .subscribe(this.OnShowScoresMessage);
  }

  private OnWordsToPick = (event: WordsToPickEvent) =>
  {
    let dialogRef: MatDialogRef<PickWordComponent> = this.dialog.open
    (
      PickWordComponent, 
      {data: {Words: event.Words.map((val) => val.word)}}
    );

    dialogRef
      .afterClosed()
      .subscribe(() => this.pickWordService.SendWordPicked(dialogRef.componentInstance.ChosenWord));
  }

  private OnShowScoresMessage = (val: ShowScoresEvent): void => 
  {
    let dialogRef: MatDialogRef<ShowScoresComponent> = this.dialog.open
    (
      ShowScoresComponent, 
      {data: {Word: val.Word, PlayerData: val.Scores}, width: "50vw", height: "70vh"}
    );

   dialogRef.afterOpened().subscribe(() => 
   {
     timer(5000).subscribe((_) => dialogRef.close());
   });   
  }
}