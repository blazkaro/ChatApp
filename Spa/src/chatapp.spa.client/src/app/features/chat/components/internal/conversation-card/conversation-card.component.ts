import { Component, Input } from '@angular/core';
import { Conversation } from '../../../models/conversation';

@Component({
  selector: 'app-conversation-card',
  standalone: false,
  templateUrl: './conversation-card.component.html',
  styleUrl: './conversation-card.component.css'
})
export class ConversationCardComponent {
  @Input() conv: Conversation = undefined!;
}
