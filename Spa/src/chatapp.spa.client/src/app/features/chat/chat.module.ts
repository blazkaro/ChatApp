import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ConversationHeaderComponent } from './components/conversation-header/conversation-header.component';
import { ConversationMessagesComponent } from './components/conversation-messages/conversation-messages.component';
import { ChatBarComponent } from './components/chat-bar/chat-bar.component';



@NgModule({
  declarations: [
    ConversationHeaderComponent,
    ConversationMessagesComponent,
    ChatBarComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    ConversationHeaderComponent,
    ConversationMessagesComponent,
    ChatBarComponent
  ]
})
export class ChatModule { }
