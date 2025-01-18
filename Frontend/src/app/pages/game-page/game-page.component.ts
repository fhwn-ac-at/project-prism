import { Component, inject, OnInit } from '@angular/core';
import { DrawableCanvasComponent } from '../../components/drawable-canvas/component/drawable-canvas.component';
import { TopBarComponent } from '../../components/top-bar/top-bar.component';
import { CountdownService } from '../../services/countdown/countdown.service';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { ActivePlayersService } from '../../services/current-players/active-players.service';
import { ChatComponent } from "../../components/chat/chat.component";
import { ActivePlayersComponent } from "../../components/active-players/active-players.component";
import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { Dialog } from '@angular/cdk/dialog';
import { PickWordComponent } from '../../components/pick-word/pick-word.component';
import { PickWordService } from '../../services/pick-word/pick-word.service';
import { WordsToPickEvent } from '../../services/pick-word/events/WordsToPick';

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
  private pickWordService: PickWordService;

  public constructor(pickWordService: PickWordService)
  {
    this.pickWordService = pickWordService;

    this.pickWordService.SubscribeOnWordToPick({next: this.OnWordsToPick})
  }

  private OnWordsToPick = (event: WordsToPickEvent) =>
  {
    let dialogRef: MatDialogRef<PickWordComponent> = this.dialog.open
    (
      PickWordComponent, 
      {data: {Words: event.Words}}
    );

    dialogRef
      .afterClosed()
      .subscribe(() => this.pickWordService.TriggerWordPicked(dialogRef.componentInstance.ChosenWord));
  }
}

