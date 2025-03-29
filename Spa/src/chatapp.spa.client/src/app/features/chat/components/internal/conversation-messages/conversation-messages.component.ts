import { Component } from '@angular/core';
import { Message } from '../../../models/message';

@Component({
  selector: 'app-conversation-messages',
  standalone: false,
  templateUrl: './conversation-messages.component.html',
  styleUrl: './conversation-messages.component.css'
})
export class ConversationMessagesComponent {
  messages: Message[] = [];
}
