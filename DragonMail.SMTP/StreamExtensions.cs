using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DragonMail.SMTP
{
    public static class StreamExtensions
    {
        public static void Write(this Stream clientStream, string strMessage)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(strMessage + "\r\n");
            Write(clientStream, buffer);
        }
        public static void Write(this Stream clientStream, byte[] buffer)
        {
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        public static string Read(this NetworkStream clientStream)
        {
            byte[] messageBytes = new byte[8192];
            int bytesRead = 0;

            var msgBldr = new StringBuilder();
            ASCIIEncoding encoder = new ASCIIEncoding();

            bytesRead = clientStream.Read(messageBytes, 0, messageBytes.Length);

            string message = encoder.GetString(messageBytes, 0, bytesRead);
            return message;
        }

        public static string ReadData(this Stream clientStream)
        {
            var reader = new StreamReader(clientStream);
            StringBuilder data = new StringBuilder();
            string line = reader.ReadLine();

            if (line != null && line != ".")
            {
                data.AppendLine(line);

                for (line = reader.ReadLine(); line != null && line != "."; line = reader.ReadLine())
                {
                    data.AppendLine(line);
                }
            }

            var message = data.ToString();
            return message;
        }
    }
}