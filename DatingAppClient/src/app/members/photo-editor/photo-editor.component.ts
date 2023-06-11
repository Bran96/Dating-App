import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Member } from 'src/app/Models/member';
import { Photo } from 'src/app/Models/photo';
import { User } from 'src/app/Models/user';
import { AccountService } from 'src/app/Services/account.service';
import { MembersService } from 'src/app/Services/members.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member | undefined;
  uploader: FileUploader | undefined;
  hasBaseDropZoneOver = false;  
  baseUrl = environment.apiUrl; // We need the baseUrl, because we gonna use this component to upload the image to our api
  user: User | undefined; // We also do need access to our user and because we need the user we need to get access to the accountService

  constructor(private accountService: AccountService, private membersService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if(user) {
          this.user = user;
        }
      }
    })
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  // Now we need to get the dropZone functionality
  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  // We need to initialize the file uploader
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    //  Another property to the uploader to ensure it works
    this.uploader.onAfterAddingAll = (file) => {
      file.withCredentials = false
    }

    // Now to indicate what are we gonna do after the file has been uploaded successfully
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const photo = JSON.parse(response); // Now we gonna set our photo
        this.member?.photos.push(photo);// Now we gonna push our new photo that we getting from our API
      }
    }
  }

  setMainPhoto(photo: Photo) {
    this.membersService.setMainPhoto(photo.id).subscribe({
      // What we do next in this case is:
      // 1. We need to update the photoUrl for the loggedIn user that we already have
      // 2. And we also need to update the member because the member containes the photo collection and turn off the one that is true to false which is the Main photo and set the new photo to main
      next: () => {
        // We wanna make sure that we have both the user and the member
        if(this.user && this.member) {
          this.user.photoUrl = photo.url;
          // The reason doing this: Our current user observable are subscribed to in other components to this user Object.
          // So when we update the Main photo here its also gonna update any other component that is displaying the users main photo
          this.accountService.setCurrentUser(this.user);
          this.member.photoUrl = photo.url;
          // We also need to update the elements inside the members photos array
          this.member.photos.forEach(p => {
            if(p.isMain) {
              p.isMain = false;
            }
            if(p.id === photo.id) {
              photo.isMain = true;
            }
          })
        }
      }
    })
  }

  deletePhoto(photoId: number) {
    this.membersService.deletePhoto(photoId).subscribe({
      next: () => {
        // Check if we have a member
        if(this.member) {
          this.member.photos = this.member.photos.filter(x => x.id != photoId); // filter is: we want to return everything from the member photos array except for the one photoId which is in the expression
        }
      }
    })
  }
}
