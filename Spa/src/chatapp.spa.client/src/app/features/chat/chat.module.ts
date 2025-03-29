import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { ChatWindowComponent } from './components/chat-window/chat-window.component';
import { ConversationsListComponent } from './components/conversations-list/conversations-list.component';
import { ChatBarComponent } from './components/internal/chat-bar/chat-bar.component';
import { ConversationCardComponent } from './components/internal/conversation-card/conversation-card.component';
import { ConversationHeaderComponent } from './components/internal/conversation-header/conversation-header.component';
import { ConversationMessagesComponent } from './components/internal/conversation-messages/conversation-messages.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';


@NgModule({
  declarations: [
    ConversationCardComponent,
    ConversationsListComponent,
    ConversationHeaderComponent,
    ConversationMessagesComponent,
    ChatBarComponent,
    ChatWindowComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  exports: [
    ConversationsListComponent,
    ChatWindowComponent
  ]
})
export class ChatModule { }
