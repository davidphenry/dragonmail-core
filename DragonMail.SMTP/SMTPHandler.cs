using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DragonMail.SMTP
{
    public class SMTPHandler
    {
        enum MessageReturn
        {
            QUIT,
            CONTINUE,
            HASDATA,
            IGNORE
        }
        public string ProcessClient(TcpClient client)
        {
            try
            {
                string smptData = HandleRequest(client.GetStream());

                return smptData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        StringBuilder lastMessage = new StringBuilder();
        string HandleRequest(NetworkStream clientStream)
        {
            clientStream.Write("220 dragonmail -- Dynamic email server");
            string messageData = null;
            while (true)
            {
                string tcpMessage = null;
                try
                {
                    tcpMessage = clientStream.Read();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //a socket error has occured
                    break;
                }

                if (tcpMessage.Length <= 0)
                    continue;

                MessageReturn result = MessageReturn.CONTINUE;

                if (tcpMessage.Length == 1)
                {
                    //interactive email being entered character at a time
                    lastMessage.Append(tcpMessage);
                    result = HandleMessage(clientStream, lastMessage.ToString());

                    if (result != MessageReturn.IGNORE)
                        lastMessage = new StringBuilder();
                }
                else
                {
                    result = HandleMessage(clientStream, tcpMessage);
                }

                if (result == MessageReturn.QUIT)
                    break;

                if (result == MessageReturn.HASDATA)
                {
                    messageData = clientStream.ReadData();
                    clientStream.Write("250 OK");
                }

            }
            return messageData;
        }

        static MessageReturn HandleMessage(NetworkStream clientStream, string tcpMessage)
        {

            if (tcpMessage.StartsWith("QUIT"))
            {
                return MessageReturn.QUIT;
            }

            if (tcpMessage.StartsWith("EHLO") || tcpMessage.StartsWith("RCPT TO") || tcpMessage.StartsWith("MAIL FROM"))
            {
                clientStream.Write("250 OK");
                return MessageReturn.CONTINUE;
            }
            else if (tcpMessage.StartsWith("DATA"))
            {
                clientStream.Write("354 Start mail input; end with");
                return MessageReturn.HASDATA;
            }
            return MessageReturn.IGNORE;
        }


    }
}
