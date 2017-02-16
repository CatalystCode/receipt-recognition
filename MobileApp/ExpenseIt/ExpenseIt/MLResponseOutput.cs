using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseIt
{
    
    public class MLResponseOutput
    {
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"clothes and accessories\"")]
        public double ClothesAndAccessoriesProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"daily snack\"")]
        public double DailySnacksProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"dining out\"")]
        public double DiningOutProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"entertinement\"")]
        public double EntertainmentProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"fuel\"")]
        public double FuelProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Probabilities for Class \"groceries\"")]
        public double GroceriesProbability { get; set; }
        [JsonProperty(PropertyName = "Scored Labels")]
        public string ScoredLabels { get; set; }
    }

    public class MLResponseResults
    {
        [JsonProperty(PropertyName = "output1")]
        public List<MLResponseOutput> Output { get; set; }
    }

    public class MLResponse
    {
        public MLResponseResults Results { get; set; }
    }
}
