import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { MembersService } from 'src/app/Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit{
  // Lets rather make it an observable of type Member array (A different approach)
  members$: Observable<Member[]> | undefined;
  // members: Member[] = [];

  constructor(private membersService: MembersService) {}

  ngOnInit(): void {
    // this.loadMembers(); // Since we're using an Observable we dont need this method.
    this.members$ = this.membersService.getMembers();
  }

  // loadMembers() { // Since we're using an Observable we dont need this method.
  //   this.membersService.getMembers().subscribe({
  //     next: (response) => {
  //       this.members = response;
  //     }
  //   })
  // }
}
