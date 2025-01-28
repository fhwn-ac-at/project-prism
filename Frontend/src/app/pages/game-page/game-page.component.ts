import { Component, inject, OnDestroy } from '@angular/core';
import { DrawableCanvasComponent } from '../../components/drawable-canvas/component/drawable-canvas.component';
import { TopBarComponent } from '../../components/top-bar/top-bar.component';
import { ChatComponent } from "../../components/chat/chat.component";
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import { PickWordComponent } from '../../components/pick-word/pick-word.component';
import { PickWordService } from '../../services/pick-word/pick-word.service';
import { WordsToPickEvent } from '../../services/pick-word/events/WordsToPick';
import { ResizeService } from '../../services/resize/resize.service';
import { ShowScoresService } from '../../services/show-scores/show-scores.service';
import { ShowScoresEvent } from '../../services/show-scores/events/ShowScoresEvent';
import { ShowScoresComponent } from '../../components/show-scores/show-scores.component';
import { Subscription, timer } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environment/environment';

@Component
(
  {
  selector: 'app-game-page',
  imports: [DrawableCanvasComponent, TopBarComponent, ChatComponent, ActivePlayersComponent],
  templateUrl: './game-page.component.html',
  styleUrl: './game-page.component.css',
  }
)
export class GamePageComponent implements OnDestroy
{
  private dialog: MatDialog = inject(MatDialog);
  private resizeService = inject(ResizeService)
  private pickWordService = inject(PickWordService);
  private showScoresService = inject(ShowScoresService);

  public CalculatedCanvasWidth: number = 0;
  public CalculatedCanvasHeight: number = 0;
  private router: Router = inject(Router);

  // subs
  private sub1: Subscription;
  private sub2: Subscription;
  
  public constructor()
  {
    this.sub1 = this.pickWordService.ObserveWordsToPickEvent()
      .subscribe(this.OnWordsToPick);

    this.resizeService.onResize$.subscribe((data) => 
    {
      // 60vw - 4% padding and 4% margin of container with 60vw
      this.CalculatedCanvasWidth=data.width*0.60 - ((data.width*0.60) *0.08); 
      
      // 80vh
      this.CalculatedCanvasHeight=data.height*0.80;
    });
    
    this.sub2 = this.showScoresService.ObserveShowScoresEvent()
      .subscribe(this.OnShowScoresMessage);   
  }

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();

    this.sub2.unsubscribe();
  }

  private OnWordsToPick = (event: WordsToPickEvent) =>
  {
    let dialogRef: MatDialogRef<PickWordComponent> = this.dialog.open
    (
      PickWordComponent, 
      {data: {Words: event.Words.map((val) => val.word)}}
    );

    dialogRef.afterOpened().subscribe(() => 
    {
      timer(environment.game.pickWordDialogDuration).subscribe((_) => dialogRef.close());
    });

    dialogRef
      .afterClosed()
      .subscribe(() => this.pickWordService.SendWordPicked(dialogRef.componentInstance.ChosenWord));
  }

  private OnShowScoresMessage = (val: ShowScoresEvent): void => 
  {
    let dialogRef: MatDialogRef<ShowScoresComponent> = this.dialog.open
    (
      ShowScoresComponent, 
      {
        data: 
        {
          Word: val.Word,
          PlayerData: val.Scores,
          IsEnded: val.IsEnded
        },
        width: "50vw",
        height: "70vh"
      }
    );

    dialogRef.afterOpened().subscribe(() => 
      {
         timer(environment.game.showScoresDialogDuration).subscribe((_) => 
         {
           dialogRef.close();
        });
      }); 

    dialogRef.afterClosed().subscribe(() => 
    {
      if (val.IsEnded)
      {
        this.router.navigate(["/lobby"]);
      }
    })
  }
}