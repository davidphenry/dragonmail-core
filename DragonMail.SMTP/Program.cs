using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace DragonMail.SMTP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RunServer(25);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        static void RunServer(int port) {
            var ipAddress = IPAddress.Any;
            
            var listener = new TcpListener(ipAddress, port);
            
            //listener.ExclusiveAddressUse = false;
            listener.Start();
            Console.WriteLine($"Server started. Listening to TCP clients at {ipAddress}:{port}");

            while (true)
            {
                if (listener.Pending())
                {
                    Console.WriteLine("Waiting for client...");
                    var connectionThread = new Thread(new ThreadStart(() =>
                    {
                        string messageData = null;
                        using (TcpClient c = listener.AcceptTcpClient())
                        {
                            Console.WriteLine("Client connected. Waiting for data.");
                            var handler = new SMTPHandler();
                            messageData = handler.ProcessClient(c);

                            Console.WriteLine("Client processed, disconnecting.");
                            c.Close();
                        }

                        if (!string.IsNullOrEmpty(messageData))
                        {
                            ISaveMail mailSaver = new SaveMailS3();
                            mailSaver.SaveMail(messageData);
                        }
                    }));

                    connectionThread.Start();
                }
            }
        }

    }
}
