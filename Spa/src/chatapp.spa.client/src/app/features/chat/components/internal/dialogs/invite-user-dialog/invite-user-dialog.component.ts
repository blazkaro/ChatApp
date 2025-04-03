import { Component } from '@angular/core';
import { User } from '../../../../../../core/models/user';
import { UserSearchService } from '../../../../services/user-search.service';

@Component({
  selector: 'app-invite-user-dialog',
  standalone: false,
  templateUrl: './invite-user-dialog.component.html',
  styleUrl: './invite-user-dialog.component.css'
})
export class InviteUserDialogComponent {
  selectedUser: User | undefined = undefined;
  filteredUsers: User[] = [];

  constructor(private readonly userSearchService: UserSearchService) { }

  filterUsers(namePrefix: string) {
    this.userSearchService.search(namePrefix).then((users: User[]) => {
      this.filteredUsers = users;
    })
  }

  displayOption(user: User) {
    return user.nickname;
  }

  selectUser(user: User | undefined) {
    this.selectedUser = user;
  }


}
