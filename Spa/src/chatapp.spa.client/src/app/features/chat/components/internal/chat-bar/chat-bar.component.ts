import { Component, Input } from '@angular/core';
import { Conversation } from '../../../models/conversation';

@Component({
  selector: 'app-chat-bar',
  standalone: false,
  templateUrl: './chat-bar.component.html',
  styleUrl: './chat-bar.component.css'
})
export class ChatBarComponent {
  @Input() conv: Conversation = undefined!;
}
