import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  validationErrors: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    
  }

  // We gonna create a bunch of methods to test various different error responses
  // Also we wont get anything in the next observer, because angular is smart enough to know that we should only get a response of 200 ok in next, however
  // we will get a response in the error observer
  // Our goal is to get the different errors we getting back from the API

    get404Error() {
      this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      })
    }

    get400Error() {
      this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      })
    }

    get500Error() {
      this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      })
    }

    get401Error() {
      this.http.get(this.baseUrl + 'buggy/auth').subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        }
      })
    }

    get400ValidationError() {
      this.http.post(this.baseUrl + 'account/register', {}).subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
          this.validationErrors = error;
        }
      })
    }
    
}
