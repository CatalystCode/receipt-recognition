using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

using static System.DateTime;
using static System.String;

namespace InvoiceIt
{
    public class Invoice
    {
        public DateTime TimeStamp { get; set; } = UtcNow;
        public string Photo { get; set; } = Empty;
        public string ScoredLabel1 { get;  set; }
        public string ScoredLabel2 { get;  set; }
        public string ScoredLabel3 { get;  set; }
    }

    public class MLOutputItem
    {
        public float Probability { get; set; }
        public string Category { get; set; }
    }
}
