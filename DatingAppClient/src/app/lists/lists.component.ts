import { Component, OnInit } from '@angular/core';
import { Member } from '../Models/member';
import { MembersService } from '../Services/members.service';
import { PaginatedResult, Pagination } from '../Models/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    // this.memberService.getLikes(this.predicate).subscribe({
    //   next: response => {
    //     this.members = response;
    //   }
    // });

    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    });
  }

  pageChanged(event: any) {
    if(this.pageNumber !== event.page) { // Its just basically to make sure that the code can be run if we are not already on that page
      this.pageNumber = event.page
      this.loadLikes(); // We need to call the loadLikes() again to get the updated list of members
    }
  }
}
