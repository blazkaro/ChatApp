import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, map } from 'rxjs';
import { Conversation } from '../models/conversation';
import { ConversationInvitation } from '../models/conversation-invitation';
import { ConversationMember } from '../models/conversation-member';
import { CreateConversation } from '../models/create-conversation';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private readonly http: HttpClient) { }

  getConversations(): Promise<Conversation[]> {
    return firstValueFrom(this.http.get<Conversation[]>('/api/conversations'));
  }

  getMembers(conversationId: string): Promise<ConversationMember[]> {
    return firstValueFrom(this.http.get<ConversationMember[]>(`/api/conversations/${conversationId}/members`));
  }

  getMessages(conversationId: string, page: number): Promise<Message[]> {
    return firstValueFrom(this.http.get<Message[]>(`/api/messages/${conversationId}?page=${page}`).pipe(
      map((messages: Message[]) =>
        messages.map((msg: Message) => ({
          ...msg,
          sentAt: new Date(msg.sentAt)
        }))
      )
    ));
  }

  createConversation(conv: CreateConversation): Promise<Conversation> {
    return firstValueFrom(this.http.post<Conversation>('/api/conversations', conv));
  }

  createConversationInvitation(conversationId: string, userId: string): Promise<{}> {
    return firstValueFrom(this.http.post<{}>(`/api/conversations/${conversationId}/invitations`, { userToInvite: userId }));
  }

  getConversationsInvitations() {
    return firstValueFrom(this.http.get<ConversationInvitation[]>("/api/conversations/invitations"));
  }

  async acceptConversationInvitation(conversationId: string): Promise<void> {
    await firstValueFrom(this.http.post<{}>(`/api/conversations/${conversationId}/invitations/accept`, {}));
  }
}
