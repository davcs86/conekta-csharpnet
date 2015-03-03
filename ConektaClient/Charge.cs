using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Charge : Resource
    {
        public int amount;
        public int created_at;
        public String currency;
        public String customer_id;
        public String description;
        public Details details;
        public String failure_code;
        public String failure_message;
        public int fee;
        public Boolean livemode;
        public int? monthly_installments;
        public int? paid_at;
        public PaymentMethod payment_method;
        public String reference_id;
        public Refunds refunds;
        public String status;

        public Charge(String id = null) : base(id)
        {
        }

        public Charge()
        {
        }

        public static Charge find(String id)
        {
            return (Charge) scpFind("charge", id);
        }

        public static ConektaObject where(JObject _params)
        {
            return scpWhere("charge", _params);
        }

        public static ConektaObject where()
        {
            return scpWhere("charge", null);
        }

        public static Charge create(JObject _params)
        {
            return (Charge) scpCreate("charge", _params);
        }

        public Charge capture()
        {
            return (Charge) customAction("POST", "capture", null);
        }

        public Charge refund(int amount)
        {
            var _params = JObject.Parse("{'amount':" + amount + "}");

            return (Charge) customAction("POST", "refund", _params);
        }

        public Charge refund()
        {
            var _params = JObject.Parse("{'amount':" + amount + "}");

            return (Charge) customAction("POST", "refund", _params);
        }
    }
}