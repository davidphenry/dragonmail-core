import { Component, OnInit } from '@angular/core';
import { DragonmailService} from './shared/dragonmail.service';
import { DSMail} from './shared/dsmail';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import 'rxjs/add/operator/switchMap';

@Component({
  selector:    'mailbox',
  templateUrl: './mailbox.component.html',
  providers:  [ DragonmailService ]
})

export class MailboxComponent implements OnInit {

  isLoading: boolean;
  
  mailBox: string;
  
  mailList : DSMail[] = [];
  
  selectedMail:DSMail =  new DSMail();
  
 constructor(
	private _dragonmailService: DragonmailService,
    private route: ActivatedRoute,
    private router: Router) {}	
  
  ngOnInit() {
	this.isLoading = true;
	 this.route.paramMap
      .switchMap((params: ParamMap) => 
        this.searchMail(params.get('name')))
		.subscribe((mail: DSMail[]) => {
			this.isLoading = false;
			this.mailList = mail
			this.selectedMail = mail[0];
		});
  }
  
  selectMailItem(mailItem) {
	this.selectedMail = mailItem;
  }
  
  searchMail(mailBox) {
		this.mailBox = mailBox;
		return this._dragonmailService.getMail(mailBox);	
  }
  
  downloadAttachment(mailItem, attachment) {
	this._dragonmailService.downloadAttachment(mailItem, attachment).then(s3file=> {
		var blob = new Blob([s3file['Body']], {type: s3file['ContentType']});
		let url = window.URL.createObjectURL(blob);
		window.open(url);		
	});
  }
}