import { Component } from '@angular/core';
import { Conversation } from '../../../../shared/models/conversation';

@Component({
  selector: 'app-conversations-list',
  standalone: false,
  templateUrl: './conversations-list.component.html',
  styleUrl: './conversations-list.component.css'
})
export class ConversationsListComponent {
  conversations: Array<Conversation> = [
    {
      id: "0",
      name: "Some Really Weird User Name",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    },
    {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    },
    {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    },
    {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    },
    {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }, {
      id: "1",
      name: "Some Even More Weird User Name. WTF cats everywhere",
      avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
    }
  ]
}
