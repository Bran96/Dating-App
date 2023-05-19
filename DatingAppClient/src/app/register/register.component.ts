import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../Services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() usersFromHomeComponent: any; // Getting data from the parent component to this(child) component
  @Output() cancelRegister = new EventEmitter(); // We want to emit(sending) something from child component to parent component
  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  ngOnInit(): void {
    
  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => {
        console.log(error);
        this.toastr.error(error.error);
      }
    })

  }

  cancel() {
    this.cancelRegister.emit(false); // Put in the emit parenthese what we want to emit, because we want to set the registrationMode in the homeComponent to false
  }

}
