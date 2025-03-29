import { Component, Input } from '@angular/core';
import { Conversation } from '../../../models/conversation';

@Component({
  selector: 'app-conversation-header',
  standalone: false,
  templateUrl: './conversation-header.component.html',
  styleUrl: './conversation-header.component.css'
})
export class ConversationHeaderComponent {
  @Input() conv: Conversation = undefined!;
}
