import { Component, Input } from '@angular/core';
import { RealTimeCommunicationService } from '../../../../../core/services/real-time-communication.service';
import { Conversation } from '../../../models/conversation';

@Component({
  selector: 'app-chat-bar',
  standalone: false,
  templateUrl: './chat-bar.component.html',
  styleUrl: './chat-bar.component.css'
})
export class ChatBarComponent {
  @Input() conv: Conversation = undefined!;

  constructor(private readonly rtcService: RealTimeCommunicationService) { }

  sendMessage(msgArea: HTMLTextAreaElement) {
    this.rtcService.callMethod('SendMessageAsync', { content: msgArea.value, conversationId: this.conv.id });
    msgArea.focus();
    msgArea.value = '';
    return false;
  }
}
