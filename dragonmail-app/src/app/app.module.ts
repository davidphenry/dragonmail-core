import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { HttpModule }    from '@angular/http';
import { DragonmailService } from './shared/dragonmail.service';

import {FlexLayoutModule} from "@angular/flex-layout";
import {Router } from '@angular/router';
import {MdProgressSpinnerModule} from '@angular/material';
import {MdToolbarModule} from '@angular/material';
import {MdInputModule} from '@angular/material';
import {MdIconModule} from '@angular/material';
import {MdButtonModule} from '@angular/material';
import {MdListModule} from '@angular/material';
import {MdCardModule} from '@angular/material';
import {MdSidenavModule} from '@angular/material';

import { AppComponent } from './app.component';
import { MailboxComponent } from './mailbox.component';


const appRoutes: Routes = [  
  { path: 'mailbox/:name', component: MailboxComponent },    
  //{ path: '**', component: AppComponent }
];


@NgModule({
  declarations: [
    AppComponent,
	MailboxComponent
  ],
  imports: [
	FlexLayoutModule,
	RouterModule.forRoot(appRoutes, { enableTracing: true}),
    BrowserModule,
	HttpModule,
    BrowserAnimationsModule, 
	
	MdToolbarModule,
	MdInputModule,
	MdIconModule,
	MdButtonModule,
	MdCardModule,
	MdSidenavModule,
	MdListModule
  ],
  providers: [DragonmailService],
  bootstrap: [AppComponent]
})
export class AppModule { 
  
  constructor(router: Router) {
    console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
  }}
