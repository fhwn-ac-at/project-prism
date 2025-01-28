import { Component } from '@angular/core';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { Maybe } from '@sweet-monads/maybe';
import { NgFor, NgIf } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { WordPart } from '../../services/hidden-word/WordPart';

@Component({
  selector: 'app-hidden-word',
  imports: [NgFor, NgIf, MatCard],
  templateUrl: './hidden-word.component.html',
  styleUrl: './hidden-word.component.css'
})
export class HiddenWordComponent 
{
  private hiddenWordService: HiddenWordService;

  public constructor(hiddenWordService: HiddenWordService)
  {
    this.hiddenWordService = hiddenWordService;

    this.hiddenWordService.ObserveWordEvent()
    .subscribe((event) => {
      this.Word = event.LettersOrNones
    }
    )
  }

  public Word: WordPart[] | undefined;
}
