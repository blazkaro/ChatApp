import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RealTimeCommunicationService } from '../../../../../../core/services/real-time-communication.service';
import { ConversationInvitation } from '../../../../models/conversation-invitation';
import { ConversationService } from '../../../../services/conversation.service';
import { CurrentUserService } from '../../../../../../core/services/current-user.service';

@Component({
  selector: 'app-invitations-list-dialog',
  standalone: false,
  templateUrl: './invitations-list-dialog.component.html',
  styleUrl: './invitations-list-dialog.component.css'
})
export class InvitationsListDialogComponent implements OnInit {
  invitations: ConversationInvitation[] = [];

  constructor(private readonly conversationService: ConversationService,
    private readonly snackBar: MatSnackBar,
    private readonly rtcService: RealTimeCommunicationService,
    private readonly currentUserService: CurrentUserService) { }

  ngOnInit(): void {
    this.conversationService.getConversationsInvitations()
      .then((invs: ConversationInvitation[]) => {
        this.invitations.push(...invs);
      });

    this.rtcService.onCall('OnConversationJoin', async (convId, userId) => {
      if (userId == (await this.currentUserService.getCurrentUser()).id) {
        let idx: number = this.invitations.findIndex(p => p.conversationId == convId);
        this.invitations.splice(idx, 1);
      }
    });
  }

  acceptInvitation(invitation: ConversationInvitation) {
    this.conversationService.acceptConversationInvitation(invitation.conversationId)
      .then(() => {
        this.snackBar.open('Invitation accepted!', undefined, {
          duration: 3000
        });
      });
  }
}
