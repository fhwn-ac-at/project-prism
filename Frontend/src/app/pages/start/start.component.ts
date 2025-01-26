import { Component, inject } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule, Validators }
 from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import {MatSnackBar} from '@angular/material/snack-bar';
import { StartService } from '../../services/start/start.service';
import { Router } from '@angular/router';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-start',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
  ],
  templateUrl: './start.component.html',
  styleUrl: './start.component.css'
})
export class StartComponent 
{
  private snackbar: MatSnackBar;

  private startService: StartService = inject(StartService);
  private router: Router = inject(Router);

  public constructor(snackbar: MatSnackBar)
  {
    this.snackbar = snackbar;
  }

  public LobbyIDToJoinModel: FormControl = new FormControl("", [Validators.required, Validators.minLength(6)]);

  public async OnJoinGameButtonClicked(_: MouseEvent) 
  {
    if (this.LobbyIDToJoinModel.invalid)
    {
      this.snackbar.open("Username and/or lobby id are not valid!","",{duration:2000});
      return;
    }

    try
    {
      await this.startService.TryJoinOrStartGame(this.LobbyIDToJoinModel.value, false);

      this.router.navigate(["/lobby"]);
    }
    catch (e: any)
    {
      this.snackbar.open("Something went wrong" + e,"",{duration:2000});
    }
  }

  public async OnCreatePrivateRoomButtonClicked($event: MouseEvent) 
  {
    try
    {
      await this.startService.TryJoinOrStartGame(uuidv4(), true);

      this.router.navigate(["/lobby"]);
    }
    catch (e: any)
    {
      this.snackbar.open("Something went wrong" + e,"",{duration:2000});
    }
  }
}