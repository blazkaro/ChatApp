import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {

  private currentUser: User | null = null;
  private currentUserPromise: Promise<User> | null = null;

  constructor(private readonly http: HttpClient) { }

  async getCurrentUser(): Promise<User> {
    if (this.currentUser !== null)
      return this.currentUser;

    if (this.currentUserPromise == null) {
      this.currentUserPromise = firstValueFrom(this.http.get<User>("/api/users/current"))
        .then(user => {
          this.currentUser = user;
          this.currentUserPromise = null;
          return user;
        });
    }

    return this.currentUserPromise;
  }
}
