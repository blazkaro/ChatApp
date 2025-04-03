import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { User } from '../../../core/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserSearchService {

  constructor(private readonly http: HttpClient) { }

  search(namePrefix: string): Promise<User[]> {
    return firstValueFrom(this.http.get<User[]>(`/api/users?namePrefix=${namePrefix}`));
  }
}
