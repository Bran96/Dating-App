import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/Models/member';
import { MembersService } from 'src/app/Services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member | undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  // When the user clicks on a user to display the user it will redirect to this component
  // We gonna be able to access the route parameter of the username from this ActivatedRoute
  constructor(private membersService: MembersService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.loadMember();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
  }

  getImages() {
    if(!this.member) {
      return [];
    }

    const imageUrls = [];
    for(const photo of this.member.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username'); // This key must be exactly the same as the key in the app-routing.module.ts
    if(!username) return;
    this.membersService.getMember(username).subscribe({
      next: (response) => {
        this.member = response;
        this.galleryImages = this.getImages();
      }
    })
  }
}
