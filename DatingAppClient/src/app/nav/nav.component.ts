import { Component, OnInit } from '@angular/core';
import { AccountService } from '../Services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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

  constructor(public accountService: AccountService, private router: Router, private toaster: ToastrService) {}

  ngOnInit(): void {
    // this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/members')
      },
      error: (error) => {
        console.log(error);
        this.toaster.error(error.error);
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
