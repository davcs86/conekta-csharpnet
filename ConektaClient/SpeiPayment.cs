using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class SpeiPayment:PaymentMethod
    {
        public String clabe;
        public String tracking_code;
        public String bank;

        public SpeiPayment(JObject jsonObject) {
            //type = "real_time_payment";
            try {
                clabe = jsonObject["clabe"].ToString();
                tracking_code = jsonObject["tracking_code"].ToString();
                bank = jsonObject["bank"].ToString();
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
        }
    }
}
