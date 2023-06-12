import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../Services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() usersFromHomeComponent: any; // Getting data from the parent component to this(child) component
  @Output() cancelRegister = new EventEmitter(); // We want to emit(sending) something from child component to parent component
  registerForm: FormGroup = new FormGroup({}); // Create formGroup for Reactive form
  bsConfig: Partial<BsDatepickerConfig> | undefined;
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.applyDatePickerConfig();
    // The user is only allowed to register if they are 18 years or older on the datepicker
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  // Initialize the form
  initializeForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      gender: ['male'], // We gonna be using radio buttons for this
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
    });

    // Custom Validator for specfic form Control
    // We wanna check if the password control's value changes, because if we check that the password and confirmpassword values are the same then the form is valid,
    // but if we still make a change to the password input then the form will be still valid and we want to prevent that using the following line of code
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  // We want to match values between 2 different inputs
  // The argument is gonna be what we're gonna match to and that will be a string and we gonna return a validator function from this
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      // We're comparing the confirmPassword with the password entered
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true} // The notMatching is what we gonna use for validatio in the html template for displaying an error message inside the "hasError" method
    }
  }

  applyDatePickerConfig() {
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD MMMM YYYY'
    }
  }

  register() {
    // console.log(this.registerForm?.value);
    debugger;
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value); // We're getting the correct format of the date from the getDateOnly method
    // "values" variable will be an object now containing all the values from the registerForm and also replace the property "dateOfBirth" with a new value that will be inserted in the "values" variable object
    const values = {...this.registerForm.value, dateOfBirth: dob};
    console.log("Values: ", values);
    // We shouldnt pass in the this.registerForm.value anymore since we created another cariable inside this method that contains new values. So add the values variable instead
    this.accountService.register(values).subscribe({
      next: (response) => {
        console.log(response);
        this.router.navigateByUrl('/members');
        // this.cancel();
      },
      error: (error) => {
        console.log(error);
        this.validationErrors = error;
        // this.toastr.error(error.error);
      }
    })
  }

  cancel() {
    this.cancelRegister.emit(false); // Put in the emit parenthese what we want to emit, because we want to set the registrationMode in the homeComponent to false
  }

  // This method is to get the date form control from the date picker and format it in a way that we only get the date and not the time with,
  // because when using a datepicker we always get date and time back as a result when submitting.
  // We gonna use this method inside our register metod
  private getDateOnly(dob: string | undefined) {
    if(!dob) {
      return;
    }
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset())).toISOString().slice(0, 10);
  }

}
