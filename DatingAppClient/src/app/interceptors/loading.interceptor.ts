import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyService } from '../Services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor { // 

  constructor(private busyService: BusyService) {}

  // If we call this intercept method, that means an http request is underway
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyService.busy(); // Which is gonna increment the busyRequestCount

    return next.handle(request).pipe(
      // Using an rxjs operator called delay, because we want to fake some delay in our application and we gonna pretend that our request is gonna take a ""second""
      delay(1000),
      // finalize method - what we gonna do after things have been completed
      finalize(() => {
        // Turning of the request once the request has been completed
        this.busyService.idle();
      })
    )
  }
}
