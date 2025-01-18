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
  private playerDataService: PlayerDataService;
  
  public constructor(chatMessages: ChatMessagesService, playerDataService: PlayerDataService)
  {
    this.chatMessagesService = chatMessages;
    this.playerDataService = playerDataService;

    this.ChatMessages = this.chatMessagesService.ChatMessages.GetItems();

    this.chatMessagesService.ChatMessages.SubscribeEvent({next: this.OnChatMessagesChanged})
  }

  public ChatMessageInputModel: string = "";

  public ChatMessages: ChatMessage[];

  public OnKeyPressed(_: globalThis.Event) 
  {
    this.chatMessagesService.ChatMessages.Push(
    {
      Username: this.playerDataService.PlayerData.value.value?.Username || "NO_USERNAME_SET", 
      Message: this.ChatMessageInputModel,
      Color: "red",
    });
    
    this.ChatMessageInputModel = "";
  }

  private OnChatMessagesChanged = (_: Event): void =>
  {
    this.ChatMessages = [...this.chatMessagesService.ChatMessages.GetItems()];
  }
}

