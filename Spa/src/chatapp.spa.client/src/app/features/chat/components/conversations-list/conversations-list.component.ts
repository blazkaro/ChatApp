import { Component } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Conversation } from '../../models/conversation';
import { ChatStateService } from '../../services/chat-state.service';

@Component({
  selector: 'app-conversations-list',
  standalone: false,
  templateUrl: './conversations-list.component.html',
  styleUrl: './conversations-list.component.css'
})
export class ConversationsListComponent {
  constructor(private readonly chatState: ChatStateService) {
    chatState.loadConversations();
  }

  getConversations(): Observable<Map<string, Conversation>> {
    return of(this.chatState.conversations)
  }

  openChat(conv: Conversation) {
    this.chatState.setActiveConversation(conv.id);
  }
}
