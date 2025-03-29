import { Component } from '@angular/core';
import { Conversation } from '../../models/conversation';
import { ChatStateService } from '../../services/chat-state.service';

@Component({
  selector: 'app-chat-window',
  standalone: false,
  templateUrl: './chat-window.component.html',
  styleUrl: './chat-window.component.css'
})
export class ChatWindowComponent {

  activeConv: Conversation | undefined;

  constructor(private readonly chatState: ChatStateService) {
    chatState.getActiveConversation().subscribe((conv) => {
      this.activeConv = conv;
    });
  }
}
