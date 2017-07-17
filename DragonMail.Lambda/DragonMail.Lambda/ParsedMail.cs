using System;
using System.Collections.Generic;
using System.Text;

namespace DragonMail.Lambda
{
    public class ParsedMail
    {
        public List<DSMail> Mail { get; set; }
        public List<ParsedAttachment> Attachments { get; set; }
        public byte[] RawMail { get; set; }
    }
}
