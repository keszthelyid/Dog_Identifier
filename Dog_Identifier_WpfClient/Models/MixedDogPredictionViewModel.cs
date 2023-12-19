using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Identifier_WpfClient.Models
{
    public class MixedDogPredictionViewModel
    {
        public byte[]? PhotoData { get; set; }
        public Dog[][]? Predictions { get; set; }
    }
}
