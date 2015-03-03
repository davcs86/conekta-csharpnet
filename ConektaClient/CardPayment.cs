using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class CardPayment : PaymentMethod
    {
        public String auth_code;
        public String brand;
        public String exp_month;
        public String exp_year;
        public String last4;
        public String name;

        public CardPayment(JObject jsonObject)
        {
            //type = "card_payment";
            try
            {
                brand = jsonObject["brand"].ToString();
                auth_code = jsonObject["auth_code"].ToString();
                last4 = jsonObject["last4"].ToString();
                exp_month = jsonObject["exp_month"].ToString();
                exp_year = jsonObject["exp_year"].ToString();
                name = jsonObject["name"].ToString();
            }
            catch (Exception e)
            {
                throw new Error(e.Message);
            }
        }
    }
}