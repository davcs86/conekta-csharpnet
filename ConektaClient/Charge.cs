using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try{
                return (Charge) scpFind("charge", id);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where(JObject _params) {
            try{
                //String className = Charge.class.getCanonicalName();
                return scpWhere("charge", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where() {
            try { 
                //String className = Charge.class.getCanonicalName();
                return scpWhere("charge", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static Charge create(JObject _params){
            try { 
                //String className = Charge.class.getCanonicalName();
                return (Charge) scpCreate("charge", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public Charge capture() {
            try { 
                return (Charge) customAction("POST", "capture", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public Charge refund(int amount){
            try { 
                JObject _params;
                try {
                    _params = JObject.Parse("{'amount':" + amount + "}");
                } catch(Exception e) {
                    throw new Exception(e.ToString());
                }
                return (Charge) customAction("POST", "refund", _params);
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        public Charge refund(){
            try{
                JObject _params;
                try {
                    _params = JObject.Parse("{'amount':" + amount + "}");
                } catch(Exception e) {
                    throw new Exception(e.ToString());
                }
                return (Charge) customAction("POST", "refund", _params);
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }
    }
}
