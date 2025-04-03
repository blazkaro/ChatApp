import { Component, ElementRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { CurrentUserService } from '../../../../../core/services/current-user.service';
import { RealTimeCommunicationService } from '../../../../../core/services/real-time-communication.service';
import { Conversation } from '../../../models/conversation';
import { ConversationMember } from '../../../models/conversation-member';
import { Message } from '../../../models/message';
import { ChatStateService } from '../../../services/chat-state.service';
import { ConversationService } from '../../../services/conversation.service';

export interface ReceiveMessageDto {
  content: string;
  senderId: string;
  conversationId: string;
  sentAt: Date;
};

@Component({
  selector: 'app-conversation-messages',
  standalone: false,
  templateUrl: './conversation-messages.component.html',
  styleUrl: './conversation-messages.component.css'
})
export class ConversationMessagesComponent implements OnInit, OnChanges {
  page: number = 0;
  messages: Message[] = [];
  messagesScrolled: boolean = false;
  allMessagesLoaded: boolean = false;

  currentUserId: string | null = null;
  @Input() conv: Conversation = undefined!;
  @ViewChild('scrollable') scrollable!: ElementRef<HTMLDivElement>;

  sendersProfiles: Map<string, ConversationMember> = new Map<string, ConversationMember>();

  constructor(private readonly currentUserService: CurrentUserService,
    private readonly chatState: ChatStateService,
    private readonly conversationService: ConversationService,
    private readonly rtcService: RealTimeCommunicationService) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.loadNextPage();
  }

  ngOnInit() {
    this.currentUserService.getCurrentUser()
      .then(user => {
        this.currentUserId = user.id;
      });

    this.rtcService.onCall('ReceiveMessageAsync', async (dto: ReceiveMessageDto) => {
      if (dto.conversationId != this.conv.id)
        return;

      this.loadSenderProfileIfNecessary(dto.senderId);
      this.messages.push({ content: dto.content, senderId: dto.senderId, sentAt: new Date(dto.sentAt) });
      this.messagesScrolled = false;
    })
  }

  onMessageViewReady() {
    if (!this.messagesScrolled) {
      this.scrollDown();
      this.messagesScrolled = true;
    }
  }

  onScroll() {
    if (this.scrollable.nativeElement.scrollTop == 0) {
      this.loadNextPage();
    }
  }

  loadNextPage() {
    if (this.allMessagesLoaded)
      return;

    this.page += 1;

    this.conversationService.getMessages(this.conv.id, this.page)
      .then(msgs => {
        if (msgs.length == 0) {
          this.allMessagesLoaded = true;
          return;
        }

        msgs.forEach((msg) => {
          this.loadSenderProfileIfNecessary(msg.senderId);
          this.messages.unshift(msg);
        });
      });
  }

  private scrollDown() {
    this.scrollable.nativeElement.scrollTop = this.scrollable.nativeElement.scrollHeight;
  }

  private loadSenderProfileIfNecessary(senderId: string) {
    if (this.sendersProfiles.has(senderId))
      return;

    this.sendersProfiles.set(senderId, this.chatState.getMember(senderId));
  }
}
