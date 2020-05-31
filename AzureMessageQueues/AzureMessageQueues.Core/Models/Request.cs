using System;
using System.Collections.Generic;
using System.Text;

namespace AzureMessageQueues.Core.Models
{
    public class Request
    {
        public string Url { get; set; }
        public bool ExtractAudio { get; set; }
        public string OutputLocation { get; set; }
    }
}
