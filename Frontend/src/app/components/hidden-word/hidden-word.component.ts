import { Component, OnDestroy } from '@angular/core';
import { HiddenWordService } from '../../services/hidden-word/hidden-word.service';
import { NgFor, NgIf } from '@angular/common';
import { MatCard } from '@angular/material/card';
import { WordPart } from '../../services/hidden-word/WordPart';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-hidden-word',
  imports: [NgFor, NgIf, MatCard],
  templateUrl: './hidden-word.component.html',
  styleUrl: './hidden-word.component.css'
})
export class HiddenWordComponent implements OnDestroy
{
  private hiddenWordService: HiddenWordService;

  private sub1: Subscription;

  public constructor(hiddenWordService: HiddenWordService)
  {
    this.hiddenWordService = hiddenWordService;

    this.sub1 = this.hiddenWordService.ObserveWordEvent()
    .subscribe((event) => {
      this.Word = event.LettersOrNones
    }
    )
  }

  public Word: WordPart[] | undefined;

  ngOnDestroy(): void 
  {
    this.sub1.unsubscribe();
  }
}
