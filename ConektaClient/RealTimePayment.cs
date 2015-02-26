using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class RealTimePayment:PaymentMethod
    {
        public String store_name;
        public String barcode;
        public String barcode_url;

        public RealTimePayment(JObject jsonObject) {
            //type = "real_time_payment";
            try {
                store_name = jsonObject["store_name"].ToString();
                barcode = jsonObject["barcode"].ToString();
                barcode_url = jsonObject["barcode_url"].ToString();
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
        }
    }
}
