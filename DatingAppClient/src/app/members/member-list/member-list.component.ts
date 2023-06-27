import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/Models/member';
import { Pagination } from 'src/app/Models/pagination';
import { User } from 'src/app/Models/user';
import { UserParams } from 'src/app/Models/userParams';
import { MembersService } from 'src/app/Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit{
  // members$: Observable<Member[]> | undefined; // Lets rather make it an observable of type Member array (A different approach)
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];

  constructor(private membersService: MembersService) {
    this.userParams = this.membersService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers(); 
    // this.members$ = this.membersService.getMembers(); // Since we're using an Observable.
  }

  loadMembers() { // Since we're using an Observable we dont need this method.
    if(this.userParams) {
      this.membersService.setUserParams(this.userParams); // Setting the userParams in the membersService when loading the members in this file whatever the user has selected
      this.membersService.getMembers(this.userParams).subscribe({
        next: (response) => {
          if(response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })
    }

  }

  resetFilters() {
      this.userParams = this.membersService.resetUserParams(); // new UserParams basically means starting from scratch withthe userparams, basically starting from the default params
      this.loadMembers();
  }

  pageChanged(event: any) {
    if(this.userParams && this.userParams?.pageNumber !== event.page) { // Its just basically to make sure that the code can be run if we are not already on that page
      this.userParams.pageNumber = event.page
      this.membersService.setUserParams(this.userParams); // We also just updating the user params in the membersService when chaging pages
      this.loadMembers(); // We need to call the loadMmebers() again to get the updated list of members
    }
  }
}
