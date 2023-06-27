import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../Models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../Models/pagination';
import { UserParams } from '../Models/userParams';
import { AccountService } from './account.service';
import { User } from '../Models/user';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  memberCache = new Map(); // Using Map gives us access to properties suchh as get and set. So we can first get and then set the key value pairs in this variable
  user: User | undefined;
  userParams: UserParams | undefined;

  constructor(private http: HttpClient, private accountService: AccountService) { // Injecting the accountService to get the user so we can add the userParams in the membersService
    // The reason why we add this is because, If we take a look at our members list and go into a single member, that means the member list gets destroyed and that is where we store our userParams, which means we lost our previous query we had
    // So what we want is, If we click on one of the users and go back to the matches we would like to remember the previous query that I had and in order to do this we need to store the UeParams in the memberService, because
    // if the component gets destroyed the service does not get destroyed
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if(user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      }
    });
  }

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  // This is when the reset Filters on the matches tab will be selected
  resetUserParams() {
    if(this.user) {
      this.userParams = new UserParams(this.user);
      return this.userParams;
    }
    return; // This means if false then return nothing from this
  }

  // We want a couple of parameters for Pagination: And remember to send this up as a query string like in the backend
  getMembers(userParams: UserParams) {
    // We want all the keys from the userParams
    // console.log("UserParams: ", userParams); // This logs all the Pagination, filtering and sorting
    // console.log(Object.values(userParams).join('-')); // This is to get the keys from the userParams that we want to store in memory
    const response = this.memberCache.get(Object.values(userParams).join('-'));
    // If we dont have something inside the response for that particular key:
    if(response) {
      return of(response);
    }
    // Our HttpParams are gonna be populated with this method now, which iis the getPaginationHeaders() method
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    // We need to get access to the full http response and not just the body and by doing that we must observe like we did here
    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => { //Add this then for keeping track whats in the memory not to load the entire time
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    )
  }

  getMember(username: string) {
    // const member = this.members.find(x => x.userName === username); // Getting our member from our members array
    // if(member) {
    //   return of(member);
    // }
    //This is to allow that when we go into the user and go back and go back to the user that it stops loading when using pagination, filtering and sorting
    console.log("Incorrect: ", this.memberCache);
    const member = [...this.memberCache.values()].reduce((arr, elem) => arr.concat(elem.result), []).find((member: Member) => member.userName == username);
      if(member) return of (member);

      return this.http.get<Member>(this.baseUrl + 'users/' + username);
    };



  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    )
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {}) // We need to atleast add an object, but in this case its just the photo that we update so we add an empty object
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  // Private methods should always go underneath public methods
  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body; // Setting the response.body we getting back from the api in the PaginatedResult result's property that we want to paginate 
        }
        // We're interesting in getting the header "Pagination" to get all the Pagination values.
        const pagination = response.headers.get("Pagination");
        if (pagination) {
          paginatedResult.pagination = JSON.parse(pagination);
        }

        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams(); // This is a class provided by angular that allow us to set query string parameters along with our Http Request

    if (pageNumber && pageSize) { // Check if we have the page and itemsPerPage
      // If we do have both, we going to set our params so that we can add the query string that goes along with the request.
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }
    return params;
  }
}
