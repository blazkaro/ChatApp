import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, of } from 'rxjs';
import { RealTimeCommunicationService } from '../../../../core/services/real-time-communication.service';
import { Conversation } from '../../models/conversation';
import { ChatStateService } from '../../services/chat-state.service';
import { ConversationService } from '../../services/conversation.service';
import { CreateConversationDialogComponent } from '../internal/dialogs/create-conversation-dialog/create-conversation-dialog.component';
import { InvitationsListDialogComponent } from '../internal/dialogs/invitations-list-dialog/invitations-list-dialog.component';
import { CurrentUserService } from '../../../../core/services/current-user.service';

@Component({
  selector: 'app-conversations-list',
  standalone: false,
  templateUrl: './conversations-list.component.html',
  styleUrl: './conversations-list.component.css'
})
export class ConversationsListComponent implements OnInit {
  constructor(private readonly chatState: ChatStateService, private readonly dialog: MatDialog,
    private readonly conversationService: ConversationService,
    private readonly snackBar: MatSnackBar,
    private readonly rtcService: RealTimeCommunicationService,
    private readonly currentUserService: CurrentUserService) {
  }

  ngOnInit(): void {
    this.chatState.loadConversations();

    this.rtcService.onCall('OnConversationJoin', async (convId: string, userId: string) => {
      if (userId == (await this.currentUserService.getCurrentUser()).id) {
        await this.chatState.loadConversations();
      }
    });
  }

  getConversations(): Observable<Map<string, Conversation>> {
    return of(this.chatState.conversations);
  }

  openChat(conv: Conversation) {
    this.chatState.setActiveConversation(conv.id);
  }

  openCreateConversationDialog() {
    const dialogRef = this.dialog.open(CreateConversationDialogComponent);
    dialogRef.beforeClosed().subscribe(async (conv: Conversation | undefined) => {
      if (conv == undefined)
        return;

      let createdConv = await this.conversationService.createConversation(conv);
      createdConv.isAdmin = true;

      this.chatState.conversations.set(createdConv.id, createdConv);
      this.chatState.setActiveConversation(createdConv.id);
      this.snackBar.open('Chat created!', undefined, {
        duration: 3000
      });
    });
  }

  openInvitationsListDialog() {
    this.dialog.open(InvitationsListDialogComponent, {
      width: '50%',
      maxWidth: '100%'
    });
  }
}
