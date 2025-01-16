import { Injectable } from '@angular/core';
import { ObservableArray } from '../../../lib/observable/ObservableArray/ObservableArray';
import { Event } from '../../../lib/observable/ObservableArray/events/Event';
import { ChatMessage } from './ChatMessage';

@Injectable({
  providedIn: 'root'
})
export class ChatMessagesService 
{
  private chatMessages: ObservableArray<ChatMessage>;

  constructor() 
  {
    this.chatMessages = new ObservableArray<ChatMessage>([]);
  }

  public get ChatMessages(): ObservableArray<ChatMessage>
  {
    return this.chatMessages;
  }
}
