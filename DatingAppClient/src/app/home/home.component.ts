import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any; 

  constructor(){}

  ngOnInit(): void {
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  // The 'event' argument is the value that comes from the child component
  cancelRegisterMode(event: boolean) {
    console.log(event);
    this.registerMode = event;
  }
}
