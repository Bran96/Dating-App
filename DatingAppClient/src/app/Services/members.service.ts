import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../Models/member';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers() {
    // The extra code is for the loading indicator not do reload the component the enntire time when going to the matches tab. Please read Notepad
    // Check the length of the members array if its greater than 0 then we going to return the members from the service and remeber we need to return an observable instead of this.members,
    // this is how we will do it:
    if(this.members.length > 0) {
      return of(this.members);
    }
    // If we do not have any members on line 20 we gona go to our api here and get them
    return this.http.get<Member[]>(this.baseUrl + "users").pipe(
      map(members => {
        this.members = members;
        // Because our component is using these members, we need to return members from here as well, otherwise it will throw an error in the component where it is fetching the members
        return members;
      })
    )
    // Now we wont have the loadingindicator displaying the whole time if we move away from the matches tab and come back
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if(member) {
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

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
}
