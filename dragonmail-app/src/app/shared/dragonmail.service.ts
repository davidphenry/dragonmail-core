import { Injectable } from '@angular/core';
import { DSMail } from './dsmail'

import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import * as AWS from "aws-sdk/global";
import * as S3 from "aws-sdk/clients/s3";

@Injectable()
export class DragonmailService {
	
	private bucketEndpoint = 'http://dragonmail-queue.s3.amazonaws.com';  
	private bucketName = 'dragonmail-queue';
	
 
  constructor(private http: Http) { }

  getMail(mailBox): Promise<DSMail[]> {
	
	var s3 = new S3({apiVersion: '2006-03-01', region: 'us-east-2'});
	
	var params = { 
		Bucket: this.bucketName,
		Prefix: 'dragonmail-dragonspears-com/' + mailBox + '/mailbox/'
	};
	
	var s3promise = s3.makeUnauthenticatedRequest('listObjects',params).promise();
		
	return s3promise
	.then(this.buildDSMail)
	.catch(this.handleError);
	}
 
	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error); // for demo purposes only
		return Promise.reject(error.message || error);
	}  
	
	private buildDSMail(s3Response) {
	
		var promises = s3Response.Contents
		.map(content=> {
			var s3 = new S3({apiVersion: '2006-03-01', region: 'us-east-2'});
			var params = { Bucket: 'dragonmail-queue', Key: content.Key };
			return s3.makeUnauthenticatedRequest('getObject',params).promise();
		});
		
		return Promise.all(promises)
		.then(values=> {
			return values.map(value => {
				var json = value['Body'].toString();
				console.log(json);
				var obj = JSON.parse(json);
				return obj;
			});
		});
	}
	
	downloadAttachment(mailItem, attachment) {
		var s3 = new S3({apiVersion: '2006-03-01', region: 'us-east-2'});
		var params = { Bucket: this.bucketName, Key: mailItem.Queue + '/' + mailItem.MessageId + '/attachments/' + attachment.AttachmentName };
		return s3.makeUnauthenticatedRequest('getObject',params).promise();
	}
  
}
