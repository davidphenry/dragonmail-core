using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DragonMail.SMTP
{
    public interface ISaveMail
    {
        void SaveMail(string messageData);
    }

    public class SaveMailS3 : ISaveMail
    {
        static string BucketName = "dragonmail-incoming";    

        public SaveMailS3()
        {

        }
        public void SaveMail(string messageData)
        {
            Console.WriteLine(messageData);
            var task = Task.Run(async () => await SaveMailAsync(messageData));
            task.Wait();
            if (task.IsFaulted)
                Console.WriteLine(task.Exception);
        }

        internal async Task SaveMailAsync(string messageData)
        {
            IAmazonS3 awsClient = null;
            try
            {
                awsClient = new AmazonS3Client(Amazon.RegionEndpoint.USEast2);
               
                var request = new PutObjectRequest()
                {
                    BucketName = BucketName,
                    Key = Guid.NewGuid().ToString(),
                    ContentBody = messageData,
                   // InputStream = new MemoryStream(Encoding.ASCII.GetBytes(messageData)),
                    CannedACL = S3CannedACL.PublicRead,
                 };

                var response = await awsClient.PutObjectAsync(request);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                if (awsClient != null)
                    awsClient.Dispose();
            }
        }
    }
}
