
export class DSMailAttachment {
	AttachmentName: string;
	ContentType: string;
	Size: number;
}

export class DSMail {
	MessageId: string;
	ToName: string;
	ToEmail: string;
	FromName: string;
	FromEmail: string;
	Content: string;
	Subject: string;
	TextBody: string;
	HtmlBody: string;
	SentDate: string;
	Queue: string;
	RawMailSize: number;
	MessageStatus: number;
	TextPreview: string;
	Attachments: DSMailAttachment[];
}
