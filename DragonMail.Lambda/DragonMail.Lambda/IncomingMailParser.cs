using Amazon.S3;
using Amazon.S3.Model;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonMail.Lambda
{
    public interface IMailParser
    {
        Task ParseMessage(byte[] message, string messageId);
        ParsedMail GenerateParsedMail(byte[] rawMail, string messageId);
    }
    public class IncomingMailParser : IMailParser
    {

        public async Task ParseMessage(byte[] message, string messageId)
        {
            var parsedMail = GenerateParsedMail(message, messageId);

            await SaveParsedMail(parsedMail);
        }
        public ParsedMail GenerateParsedMail(byte[] rawMail, string messageId)
        {
            MimeMessage mimeMessage;
            mimeMessage = MimeMessage.Load(new MemoryStream(rawMail));

            var parsedMail = mimeMessage.MimeMessageToDSMail(messageId);
            parsedMail.RawMail = rawMail;
            return parsedMail;
        }
        internal async Task SaveParsedMail(ParsedMail parsedMail)
        {

            string bucketName = System.Environment.GetEnvironmentVariable("queueName");
            //string accessKey = System.Environment.GetEnvironmentVariable("queueAccessKey");
            //string secretAccessKey = System.Environment.GetEnvironmentVariable("queueSecretKey");


            using (IAmazonS3 awsClient = new AmazonS3Client(Amazon.RegionEndpoint.USEast2))
            {
                foreach (var message in parsedMail.Mail)
                {
                    string messageKey = string.Format("{0}/{1}", message.Queue, message.MessageId);
                    message.RawMailSize = parsedMail.RawMail.Length;

                    //save message
                    var request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = $"{ message.Queue}/mailbox/{message.MessageId}",
                        ContentBody = JsonConvert.SerializeObject(message),
                        CannedACL = S3CannedACL.PublicRead,                        
                    };
                    var response = await awsClient.PutObjectAsync(request);

                    request = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        InputStream = new MemoryStream(parsedMail.RawMail),
                        ContentType = "application/octect-stream",
                        Key = $"{messageKey}/raw.bytes",
                        CannedACL = S3CannedACL.PublicRead
                    };
                    response = await awsClient.PutObjectAsync(request);

                    if (!parsedMail.Attachments.Any())
                        continue;

                    foreach (var attachment in parsedMail.Attachments)
                    {
                        request = new PutObjectRequest()
                        {
                            BucketName = bucketName,
                            InputStream = new MemoryStream(attachment.File),
                            Key = $"{messageKey}/attachments/{attachment.Name}",
                            ContentType = attachment.ContentType,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        response = await awsClient.PutObjectAsync(request);
                    }


                }
            }
        }
    }
}
