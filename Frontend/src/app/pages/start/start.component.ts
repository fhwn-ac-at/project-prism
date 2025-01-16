import { Component } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule, Validators }
 from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import {MatSnackBar} from '@angular/material/snack-bar';

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

  public constructor(snackbar: MatSnackBar)
  {
    this.snackbar = snackbar;
  }

  public UsernameModel: FormControl = new FormControl("",[Validators.required, Validators.minLength(4)]);

  public LobbyIDToJoinModel: FormControl = new FormControl("", [Validators.required, Validators.minLength(6)]);

  public OnJoinGameButtonClicked(_: MouseEvent) 
  {
    if (this.UsernameModel.invalid || this.LobbyIDToJoinModel.invalid)
    {
      this.snackbar.open("Username and/or lobby id are not valid!","",{duration:2000});
      return;
    }
  }

  public OnCreatePrivateRoomButtonClicked($event: MouseEvent) 
  {
    if (this.UsernameModel.invalid)
    {
      this.snackbar.open("Username is not valid!","", {duration:2000});
      return;
    }
  }
}
