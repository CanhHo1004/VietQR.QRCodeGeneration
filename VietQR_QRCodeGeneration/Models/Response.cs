using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VietQR_QRCodeGeneration.Models
{
    public class Response
    {
        public int resultCode { get; set; }
        public string resultString { get; set; }
        public object resultData { get; set; }
    }
}