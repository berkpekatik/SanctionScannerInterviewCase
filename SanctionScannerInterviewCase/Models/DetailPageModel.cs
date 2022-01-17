using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Models
{
    public class DetailPageModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }//Enum yapılabilir.
        public string City { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public List<AttributeModel> AttributeList { get; set; }
    }
}
