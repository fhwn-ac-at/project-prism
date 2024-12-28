import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { JsonPipe } from '@angular/common';
import { PlayerData } from '../../services/player-data/PlayerData';
import { PlayerType } from '../../services/player-data/PlayerType';

@Component({
  selector: 'app-home',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, JsonPipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent 
{

  public Data: PlayerData =  { Username: "TestUser", Role: PlayerType.Drawer};
}
