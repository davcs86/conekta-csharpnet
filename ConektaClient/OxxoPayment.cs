using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class OxxoPayment : PaymentMethod
    {
        public String barcode;
        public String barcode_url;
        public int expires_at;

        public OxxoPayment(JObject jsonObject)
        {
            //this.type = "cash_payment";
            try
            {
                barcode = jsonObject["barcode"].ToString();
                barcode_url = jsonObject["barcode_url"].ToString();
                expires_at = jsonObject["expires_at"].ToObject<int>();
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
