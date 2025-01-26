import { Component } from '@angular/core';
import { ChatMessagesService } from '../../services/chat-messages/chat-messages.service';
import { ChatMessage } from '../../services/chat-messages/ChatMessage';
import { Event } from '../../../lib/observable/ObservableArray/events/Event';
import { MatCardModule } from '@angular/material/card';
import {MatFormFieldModule} from "@angular/material/form-field"
import {MatInputModule} from "@angular/material/input"
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { PlayerDataService } from '../../services/player-data/player-data.service';
import {MatListModule} from "@angular/material/list"
import { NgStyle } from '@angular/common';



@Component({
  selector: 'app-chat',
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, MatListModule, NgStyle],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent 
{
  private chatMessagesService: ChatMessagesService;
  
  public constructor(chatMessages: ChatMessagesService)
  {
    this.chatMessagesService = chatMessages;

    this.ChatMessages = this.chatMessagesService.ChatMessages;

    this.chatMessagesService.ObserveChatMessages().subscribe({next: this.OnChatMessagesChanged});
  }

  public ChatMessageInputModel: string = "";

  public ChatMessages: ChatMessage[];

  public OnKeyPressed(_: globalThis.Event) 
  {   
    if(this.ChatMessageInputModel == "")
    {
      return;
    }

    this.chatMessagesService.AddAndSendChatMessage(this.ChatMessageInputModel);

    this.ChatMessageInputModel = "";
  }

  private OnChatMessagesChanged = (_: ChatMessage): void =>
  {
    this.ChatMessages = this.chatMessagesService.ChatMessages;
  }
}

