using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoclogixTask.Models
{
    public class Record
    {
        public string signatureId { get; set; }
        public string deviceVendor { get; set; }
        public string deviceProduct { get; set; }
        public string deviceVersion { get; set; }
        public int severity { get; set; }
        public string name { get; set; }
        public string start { get; set; }
        public string rt { get; set; }
        public string msg { get; set; }
        public string shost { get; set; }
        public string smac { get; set; }
        public string dhost { get; set; }
        public string dmac { get; set; }
        public string suser { get; set; }
        public string suid { get; set; }
        public string externalId { get; set; }
        public string cs1Label { get; set; }
        public string cs1 { get; set; }
    }
}
