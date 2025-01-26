import { Component, inject } from '@angular/core';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import { PlayerType } from '../../services/player-data/PlayerType';
import { LobbyService } from '../../services/lobby/lobby.service';
import { MatCard } from '@angular/material/card';
import { MatInput, MatLabel } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDivider } from '@angular/material/divider';
import { MatButton } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LobbyUserType } from '../../services/lobby-user-type/LobbyUserType';
import { LobbyUserTypeService } from '../../services/lobby-user-type/lobby-user-type.service';

@Component({
  selector: 'app-lobby-options',
  imports: [MatCard, MatInput, MatLabel, MatFormFieldModule, FormsModule, MatDivider, MatButton, ReactiveFormsModule],
  templateUrl: './lobby-options.component.html',
  styleUrl: './lobby-options.component.css'
})
export class LobbyOptionsComponent 
{
  private snackbar: MatSnackBar;
  
  public constructor
  (
    lS: LobbyUserTypeService,
    lO: LobbyService,
    snackBar: MatSnackBar
  )
  {
    this.LobbyUserTypeService = lS;
    this.LobbyOptionsService = lO;
    this.snackbar = snackBar;

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

  public LobbyUserTypeService: LobbyUserTypeService;
  public LobbyOptionsService: LobbyService;
  public OptionsData: FormGroup;

  public LobbyUserType: typeof LobbyUserType = LobbyUserType;

  public async OnStartGameButtonClicked(_: MouseEvent) 
  {
    this.LobbyOptionsService
    .StartGame()
    .then
    (
      () => { this.snackbar.open("Game started","", {duration: 2000}); },
      (err) => { this.snackbar.open("Failed to start game: " + err, "", {duration: 2000}); }
    );
  }

  public OnRoundsAmountChanged(_: any) 
  {
    if (this.OptionsData.controls["roundsAmount"].invalid) return;

    this.LobbyOptionsService.RoundAmount.next(this.OptionsData.controls["roundsAmount"].value);
  }

  public OnDurationChanged(_: any) 
  {
    if (this.OptionsData.controls["roundsAmount"].invalid) return;

    this.LobbyOptionsService.RoundAmount.next(this.OptionsData.controls["roundsAmount"].value);
  }
}
