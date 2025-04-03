import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User } from '../../../../../core/models/user';
import { Conversation } from '../../../models/conversation';
import { ConversationService } from '../../../services/conversation.service';
import { InviteUserDialogComponent } from '../dialogs/invite-user-dialog/invite-user-dialog.component';

@Component({
  selector: 'app-conversation-header',
  standalone: false,
  templateUrl: './conversation-header.component.html',
  styleUrl: './conversation-header.component.css'
})
export class ConversationHeaderComponent {
  @Input() conv: Conversation = undefined!;

  constructor(private readonly dialog: MatDialog,
    private readonly conversationService: ConversationService,
    private readonly snackBar: MatSnackBar) { }

  openInviteUserDialog() {
    const dialogRef = this.dialog.open(InviteUserDialogComponent);
    dialogRef.beforeClosed().subscribe(async (userToInvite: User | undefined) => {
      if (userToInvite == undefined)
        return;

      await this.conversationService.createConversationInvitation(this.conv.id, userToInvite.id);
      this.snackBar.open('User invited!', undefined, {
        duration: 3000
      });
    });
  }
}
