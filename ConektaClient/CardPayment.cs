using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class CardPayment : PaymentMethod 
    {
        public String brand;
        public String auth_code;
        public String last4;
        public String exp_month;
        public String exp_year;
        public String name;

        public CardPayment(JObject jsonObject) {
            //type = "card_payment";
            try {
                brand = jsonObject["brand"].ToString();
                auth_code = jsonObject["auth_code"].ToString();
                last4 = jsonObject["last4"].ToString();
                exp_month = jsonObject["exp_month"].ToString();
                exp_year = jsonObject["exp_year"].ToString();
                name = jsonObject["name"].ToString();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }
    }
}
