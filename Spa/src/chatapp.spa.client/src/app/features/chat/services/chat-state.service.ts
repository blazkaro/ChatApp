import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { Conversation } from '../models/conversation';

@Injectable({
  providedIn: 'root'
})
export class ChatStateService {

  constructor() { }

  private activeIdSubject: BehaviorSubject<string | null> = new BehaviorSubject<string | null>(null);

  public conversations: Map<string, Conversation> = new Map();

  loadConversations() {
    this.conversations.set("0",
      {
        id: "0",
        name: "Some Even More Weird User Name. WTF cats everywhere",
        avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
      });

    this.conversations.set("1",
      {
        id: "1",
        name: "Some Even More Weird User Name",
        avatarUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4d/Cat_November_2010-1a.jpg/640px-Cat_November_2010-1a.jpg"
      });
  }

  setActiveConversation(id: string) {
    if (this.conversations.has(id))
      this.activeIdSubject.next(id);
  }

  getActiveConversation(): Observable<Conversation | undefined> {
    return this.activeIdSubject.asObservable().pipe(
      map((id) => {
        if (id == null)
          return undefined;

        return this.conversations.get(id);
      })
    );
  }
}
