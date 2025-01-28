import { Component, OnDestroy } from '@angular/core';
import { LobbyService } from '../../services/lobby/lobby.service';
import { MatCard } from '@angular/material/card';
import { MatInput, MatLabel } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDivider } from '@angular/material/divider';
import { MatButton } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LobbyUserType } from '../../services/lobby-user-type/LobbyUserType';
import { LobbyUserTypeService } from '../../services/lobby-user-type/lobby-user-type.service';
import { Subscription } from 'rxjs';
import { environment } from '../../../environment/environment';

@Component({
  selector: 'app-lobby-options',
  imports: [MatCard, MatInput, MatLabel, MatFormFieldModule, FormsModule, MatDivider, MatButton, ReactiveFormsModule],
  templateUrl: './lobby-options.component.html',
  styleUrl: './lobby-options.component.css'
})
export class LobbyOptionsComponent implements OnDestroy
{
  private snackbar: MatSnackBar;

  private sub1: Subscription;
  private sub2: Subscription

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
        roundsAmount: new FormControl(environment.lobbyDefaults.roundAmount),
        roundDuration: new FormControl(environment.lobbyDefaults.roundDuration),
      }
    );

    this.sub1 = this.LobbyOptionsService.ObserveRoundAmount().subscribe((val) => this.OptionsData
    .setValue({roundsAmount: val, roundDuration: this.OptionsData.value.roundDuration}));

    this.sub2 =this.LobbyOptionsService.ObserveRoundDuration().subscribe((val) => this.OptionsData
    .setValue({roundsAmount: this.OptionsData.value.roundsAmount, roundDuration: val}));

    this.LobbyOptionsService.SendRoundAmount(environment.lobbyDefaults.roundAmount);
    this.LobbyOptionsService.SendRoundDuration(environment.lobbyDefaults.roundDuration);
  }

  public LobbyUserTypeService: LobbyUserTypeService;
  public LobbyOptionsService: LobbyService;
  public OptionsData: FormGroup;

  public LobbyUserType: typeof LobbyUserType = LobbyUserType;

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();
    this.sub2.unsubscribe();
  }

  public async OnStartGameButtonClicked(_: MouseEvent) 
  {
    this.LobbyOptionsService
    .StartGame()
    .catch(((err) => { this.snackbar.open("Failed to start game: are enough players present?", "", {duration: 2000}); }))
  }

  public async OnRoundsAmountChanged(_: any) 
  {
    if (this.OptionsData.controls["roundsAmount"].invalid) return;
    
    this.LobbyOptionsService.SendRoundAmount(this.OptionsData.controls["roundsAmount"].value);
  }

  public OnDurationChanged(_: any) 
  {
    if (this.OptionsData.controls["roundDuration"].invalid) return;

    this.LobbyOptionsService.SendRoundDuration(this.OptionsData.controls["roundDuration"].value);
  }
}
