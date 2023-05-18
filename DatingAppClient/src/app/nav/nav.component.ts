import { Component, OnInit } from '@angular/core';
import { AccountService } from '../Services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // We need to create a property what the user stores in the login form
  // When we access this object we gonna use model.username and model.password
  model: any = {};
  // loggedIn = false; // Whether the user is logged in or not

  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    // this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  logout() {
    this.accountService.logout();
  }
}
