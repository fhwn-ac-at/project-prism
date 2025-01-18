import { Component, inject, input, InputSignal } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-pick-word',
  imports: [MatButton, MatDialogModule],
  templateUrl: './pick-word.component.html',
  styleUrl: './pick-word.component.css'
})
export class PickWordComponent 
{
  public constructor()
  {
    const data: {Words: string[]} = inject(MAT_DIALOG_DATA);

    if (data.Words.length < 1) throw new Error("Must contain at least 1");

    this.Words = data.Words;
    this.ChosenWord = this.Words[0];  
  }

  public Words: string[];
  public ChosenWord: string;

  public OnWordClicked(word: string) 
  {
    this.ChosenWord = word;
  }
}
