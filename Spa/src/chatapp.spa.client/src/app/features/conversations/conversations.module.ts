import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ConversationCardComponent } from './components/conversation-card/conversation-card.component';
import { ConversationsListComponent } from './components/conversations-list/conversations-list.component';

@NgModule({
  declarations: [
    ConversationCardComponent,
    ConversationsListComponent
  ],
  imports: [
    CommonModule,
    MatCardModule
  ],
  exports: [
    ConversationsListComponent
  ]
})
export class ConversationsModule { }
