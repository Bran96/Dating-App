import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MemberListComponent } from './members/member-list/member-list.component';
import { HomeComponent } from './home/home.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './guards/authGuard';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';

const routes: Routes = [
  // All the routes in the aapplication
  {path: '', component: HomeComponent}, // Home Component will be a blank path since this is our first component showing up, eg. If we have an empty route its going to load the Home Component
  // This children array is just to say that instead of using one path at a time to add "canActivate: [AuthGuard]" to, we can add all of them together as AuthGuards instead.
  // This means that no one has access to those links if they are not logged into the system.
  {path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'members/:id', component: MemberDetailComponent}, // So this :id represents a route parameter, eg. members/4
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},      
    ]},
    // We dont need this route to be authenticated, you can access this route without having to login
  {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'}, // The '**' represents a path that doesnt exist in this Routes array that the user enters, eg. If the user put in an invalid route it will go to the default component that we specified as Home Component.
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
