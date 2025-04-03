import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { RealTimeCommunicationService } from '../../../core/services/real-time-communication.service';
import { Conversation } from '../models/conversation';
import { ConversationMember } from '../models/conversation-member';
import { ConversationService } from './conversation.service';

@Injectable({
  providedIn: 'root'
})
export class ChatStateService {

  conversations: Map<string, Conversation> = new Map();
  private activeConvIdSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);
  private activeConvMembers: Map<string, ConversationMember> = new Map();

  constructor(private readonly conversationService: ConversationService,
    private readonly rtcService: RealTimeCommunicationService) {
    this.Init();
  }

  Init(): void {
    this.rtcService.onCall('OnConversationJoin', async (convId: string, userId: string) => {
      if (convId == this.activeConvIdSubject.value) {
        // TODO: should update members
        // reloads all members, done just for simplicity
        this.activeConvMembers = new Map((await this.conversationService.getMembers(convId)).map(member => [member.id, member]));
      }
    });
  }

  async loadConversations(): Promise<void> {
    let convs = await this.conversationService.getConversations();
    this.conversations = new Map(convs.map(val => [val.id, val]));
  }

  async setActiveConversation(id: string): Promise<void> {
    if (this.conversations.has(id) && id !== this.activeConvIdSubject.value) {
      this.activeConvMembers = new Map((await this.conversationService.getMembers(id)).map(member => [member.id, member]));
      this.activeConvIdSubject.next(id);
    }
  }

  getActiveConversation(): Observable<Conversation | undefined> {
    return this.activeConvIdSubject.pipe(
      map((id) => {
        if (id == null)
          return undefined;

        return this.conversations.get(id);
      })
    );
  }

  getMember(id: string): ConversationMember {
    if (!this.activeConvMembers.has(id))
      throw new Error("The member id not found");

    return this.activeConvMembers.get(id)!;
  }
}
