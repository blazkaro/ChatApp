import { Component } from '@angular/core';
import { CreateConversation } from '../../../../models/create-conversation';

@Component({
  selector: 'app-create-conversation-dialog',
  standalone: false,
  templateUrl: './create-conversation-dialog.component.html',
  styleUrl: './create-conversation-dialog.component.css'
})
export class CreateConversationDialogComponent {
  conv: CreateConversation = { name: null!, avatarUrl: null! };
} 
