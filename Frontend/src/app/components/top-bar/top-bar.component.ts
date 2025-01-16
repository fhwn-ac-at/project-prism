import { Component } from '@angular/core';
import { MatCard, MatCardModule } from '@angular/material/card';
import { CountdownComponent } from "../countdown/view/component/countdown.component";
import { HiddenWordComponent } from "../hidden-word/hidden-word.component";

@Component({
  selector: 'app-top-bar',
  imports: [MatCardModule, CountdownComponent, HiddenWordComponent],
  templateUrl: './top-bar.component.html',
  styleUrl: './top-bar.component.css'
})
export class TopBarComponent 
{

}
