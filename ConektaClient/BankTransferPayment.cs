using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class BankTransferPayment:PaymentMethod
    {
        public String service_name;
        public String service_number;
        public String reference;

        public BankTransferPayment(JObject jsonObject) {
            //this.type = "bank_transfer_payment";
            try {
                service_name = jsonObject["service_name"].ToString();
                service_number = jsonObject["service_number"].ToString();
                reference = jsonObject["reference"].ToString();
            } catch (Exception e) {
                throw new Error(e.ToString());
            }
        }
    }
}
