import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/account.service';
import { MembersService } from 'src/app/Services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  // Inside the parentheses we specify the event we're listening to. Now this will be a browser event. Read notes if unsure
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
    if(this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null; // We use the accountService to populate the User's Object here

  // We're using the accountService to get the loggedIn user
  // And we also need the member service to get the member's profile for the specific user
  // providing a toast to the user to tell the user that the profile has been updated
  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    // We do not continueously have to subscribe to this so we use ".pipe(take(1)) since were using an observable called "currentUser$"
    // ToastrService - So with this "take(1)" we do not have to unsubscribe
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        this.user = user;
      }
    })
  }

  ngOnInit(): void {
    console.log("User: ", this.user);
    this.loadMember();
  }

  loadMember() {
    if(!this.user) { // Check if we have the user first of all, since we cannot proceed without the username.
      return;
    }

    this.memberService.getMember(this.user.username).subscribe({
      next: member => {
        this.member = member;
        console.log("Member: ", this.member);
      }
    })
  }

  updateMember() {
    console.log("Updated Member: ", this.member);
    console.log("Updated Member: ", this.editForm?.value);
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: response => {
        // After the profile has been updated we will display the toastr
        this.toastr.success("Profile updated successfully");
        this.editForm?.reset(this.member) // So the form is gonna be updated according to the user's current profile after save
      }
    })    
  }
}
