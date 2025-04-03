import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ChatWindowComponent } from './components/chat-window/chat-window.component';
import { ConversationsListComponent } from './components/conversations-list/conversations-list.component';
import { ChatBarComponent } from './components/internal/chat-bar/chat-bar.component';
import { ConversationCardComponent } from './components/internal/conversation-card/conversation-card.component';
import { ConversationHeaderComponent } from './components/internal/conversation-header/conversation-header.component';
import { ConversationMessagesComponent } from './components/internal/conversation-messages/conversation-messages.component';
import { CreateConversationDialogComponent } from './components/internal/dialogs/create-conversation-dialog/create-conversation-dialog.component';
import { InviteUserDialogComponent } from './components/internal/dialogs/invite-user-dialog/invite-user-dialog.component';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { InvitationsListDialogComponent } from './components/internal/dialogs/invitations-list-dialog/invitations-list-dialog.component';


@NgModule({
  declarations: [
    ConversationCardComponent,
    ConversationsListComponent,
    ConversationHeaderComponent,
    ConversationMessagesComponent,
    ChatBarComponent,
    ChatWindowComponent,
    CreateConversationDialogComponent,
    InviteUserDialogComponent,
    InvitationsListDialogComponent
  ],
  imports: [
    CommonModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    FormsModule,
    MatSnackBarModule,
    MatAutocompleteModule
  ],
  exports: [
    ConversationsListComponent,
    ChatWindowComponent
  ]
})
export class ChatModule { }
