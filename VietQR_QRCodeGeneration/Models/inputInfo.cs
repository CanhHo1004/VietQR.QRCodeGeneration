using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietQR_QRCodeGeneration.Models
{
    public class inputInfo
    {
        public string bankCode { get; set; }
        public string accountNumber { get; set; }
        public string amount { get; set; }
        public string contentBilling { get; set; }
    }
}