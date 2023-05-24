import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  // router -> so we can redirect the user if we nneed to on the error we get back from the API
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Inorder to modiy or transfrom the observable we need to use the pipe method from rxjs
    return next.handle(request).pipe(
      // We want to catch any errors inside this interceptor
      catchError((error: HttpErrorResponse) => {
        // Check if we have an error
        if(error) {
          // Now im going to switch based on the status code of the error
          switch(error.status) {
            case 400: 
              if(error.error.errors) {
                const modelStateErrors = [];
                for(const key in error.error.errors) {
                  if(error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat(); // UUse flat to make the 2 arrays as 1 single array
              } else {
                this.toastr.error(error.error, error.status.toString())
              }
              break
            case 401: 
              this.toastr.error('Unauthorised', error.status.toString());
              break;
            case 404: 
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtras: NavigationExtras = {state: {error: error.error}};
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;
            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
              break;
          }
        }
        throw error;
      })
    )
  }
}
