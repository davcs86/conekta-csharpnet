using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Charge:Resource
    {
        public Boolean livemode;
        public int created_at;
        public String status;
        public String currency;
        public String description;
        public String reference_id;
        public String failure_code;
        public String failure_message;
        public int amount;
        public PaymentMethod payment_method;
        public Details details;
        public int fee;
        public int? monthly_installments;
        public int? paid_at;
        public String customer_id;
        public Refunds refunds;

        public Charge(String id=null):base(id) {}

        public Charge() : base() {}
        
        public static Charge find(String id) {
            return (Charge) scpFind("charge", id);
        }

        public static ConektaObject where(JObject _params) {
            return scpWhere("charge", _params);
        }

        public static ConektaObject where() {
            return scpWhere("charge", null);
        }

        public static Charge create(JObject _params){
            return (Charge) scpCreate("charge", _params);
        }

        public Charge capture() {
            return (Charge) customAction("POST", "capture", null);
        }

        public Charge refund(int amount){
            JObject _params = JObject.Parse("{'amount':" + amount + "}");
            
            return (Charge) customAction("POST", "refund", _params);
        }

        public Charge refund(){
            JObject _params = JObject.Parse("{'amount':" + amount + "}");
            
            return (Charge) customAction("POST", "refund", _params);
        }
    }
}
