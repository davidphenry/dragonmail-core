using System;
using System.Collections.Generic;
using System.Text;
using MimeKit;

namespace DragonMail.Lambda
{
    public class DSMail
    {
        public DSMail() { }
        public string MessageId { get; set; }

        public string ToName { get; set; }
        public string ToEmail { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Content { get; set; }
        public string Subject { get; set; }
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public DateTime? SentDate { get; set; }
        public string Queue { get; set; }
        public Dictionary<string, int> Attachments { get; set; }
        public int RawMailSize { get; set; }

        public int MessageStatus { get; set; }
        public static string MessageQueue(string email)
        {
            if (string.IsNullOrEmpty(email))
                return string.Empty;

            string[] queueParts = email.ToLower().Split('@');
            return string.Format("{0}/{1}", queueParts[1].Replace('.', '-'), queueParts[0].Replace('.', '-'));
        }

        public string TextPreview { get; set; }


        public string SubjectDisplay()
        {
            return string.IsNullOrEmpty(Subject) ? "[No Subject]" : Subject;
        }

        internal void SetMimeInfo(MimeMessage mimeMessage)
        {
            Subject = mimeMessage.Subject;
            HtmlBody = mimeMessage.HtmlBody;
            TextBody = mimeMessage.TextBody;

            if (string.IsNullOrEmpty(TextBody))
                TextPreview = "NB";
            else if (TextBody.Length > 100)
                TextPreview = TextBody.Substring(0, 99);
            else
                TextPreview = TextBody;
        }
    }
}
