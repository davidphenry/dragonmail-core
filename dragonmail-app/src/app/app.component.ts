import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  isDefaultActive: boolean;
  
   constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}
  
  ngOnInit() {
	this.isDefaultActive = this.router.url === '/';	
  }
  
  selectMailbox(mailBox) {
	this.isDefaultActive = false;
	this.router.navigate(['/mailbox', mailBox]);
  }
  
}
