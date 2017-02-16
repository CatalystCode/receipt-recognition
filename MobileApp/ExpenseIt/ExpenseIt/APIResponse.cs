using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseIt
{
    class APIResponse
    {
        [JsonProperty(PropertyName = "receipt_url")]
        public string Receipt_Url { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
    }
}
