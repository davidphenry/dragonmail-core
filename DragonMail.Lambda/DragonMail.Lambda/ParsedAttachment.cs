using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMail.Lambda
{
    public class ParsedAttachment
    {
        public ParsedAttachment(string name, byte[] file, string contentType)
        {
            Name = name;
            File = file;
            ContentType = contentType;
        }
        public string Name { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
    }
}
