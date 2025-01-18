import { Component, inject } from '@angular/core';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { PlayerType } from '../../services/player-data/PlayerType';
import { LobbyOptionsService } from '../../services/lobby-options/lobby-options.service';
import { MatCard } from '@angular/material/card';
import { MatInput, MatLabel } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDivider } from '@angular/material/divider';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-lobby-options',
  imports: [MatCard, MatInput, MatLabel, MatFormFieldModule, FormsModule, MatDivider, MatButton, ReactiveFormsModule],
  templateUrl: './lobby-options.component.html',
  styleUrl: './lobby-options.component.css'
})
export class LobbyOptionsComponent 
{
  public constructor(pD: PlayerDataService, lO: LobbyOptionsService)
  {
    this.PlayerDataService = pD;
    this.LobbyOptionsService = lO;

    this.OptionsData = new FormGroup
    (
      {
        roundsAmount: new FormControl(this.LobbyOptionsService.RoundAmount),
        roundDuration: new FormControl(this.LobbyOptionsService.RoundDuration),
      }
    );

    this.LobbyOptionsService.RoundAmount.subscribe((val) => this.OptionsData
    .setValue({roundsAmount: val, roundDuration: this.OptionsData.value.roundDuration}));

    this.LobbyOptionsService.RoundDuration.subscribe((val) => this.OptionsData
    .setValue({roundsAmount: this.OptionsData.value.roundsAmount, roundDuration: val}));
  }

  public PlayerDataService: PlayerDataService;
  public LobbyOptionsService: LobbyOptionsService;
  public PlayerType: typeof PlayerType = PlayerType;
  public OptionsData: FormGroup;

  public OnStartGameButtonClicked(_: MouseEvent) 
  {  
    this.LobbyOptionsService.StartGame();
  }

  public OnRoundsDurationChanged($event: Event) 
  {
    this.LobbyOptionsService.RoundDuration.next(this.OptionsData.value.roundDuration);
  }

  public OnRoundsAmountChanged($event: Event) 
  {
    this.LobbyOptionsService.RoundAmount.next(this.OptionsData.value.roundsAmount);
  }
}
