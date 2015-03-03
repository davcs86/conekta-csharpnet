using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class RealTimePayment : PaymentMethod
    {
        public String barcode;
        public String barcode_url;
        public String store_name;

        public RealTimePayment(JObject jsonObject)
        {
            //type = "real_time_payment";
            try
            {
                store_name = jsonObject["store_name"].ToString();
                barcode = jsonObject["barcode"].ToString();
                barcode_url = jsonObject["barcode_url"].ToString();
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
        }
    }
}