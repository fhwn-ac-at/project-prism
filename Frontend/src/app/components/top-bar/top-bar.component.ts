import { Component } from '@angular/core';
import { MatCard, MatCardModule } from '@angular/material/card';
import { CountdownComponent } from "../countdown/view/component/countdown.component";
import { HiddenWordComponent } from "../hidden-word/hidden-word.component";
import { RoundDisplayComponent } from "../round-display/round-display.component";
import { GameIdComponent } from "../game-id/game-id.component";
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-top-bar',
  imports: [MatDivider, MatCardModule, CountdownComponent, HiddenWordComponent, RoundDisplayComponent, GameIdComponent],
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.css'
})
export class TopBarComponent 
{}
