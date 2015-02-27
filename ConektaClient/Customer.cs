using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConektaCSharp
{
    public class Customer:Resource
    {
        public ConektaObject cards;
        public Subscription subscription;
        public Boolean livemode;
        public int created_at;
        public String name;
        public String email;
        public String phone;
        public String default_card_id;
        public Boolean deleted;

        public Customer(String id=null):base(id) {
            cards = new ConektaObject();
            subscription = new Subscription();
        }

        public Customer():base(){
            cards = new ConektaObject();
            subscription = new Subscription();
        }

        public override void LoadFromObject(JObject jsonObject) {
            try { 
                if (jsonObject != null) {
                    try {
                        base.LoadFromObject(jsonObject);
                    } catch (Exception e) {
                        throw new Error(e.ToString());
                    }
                }
                for (int i = 0; i < cards.Count; i++) {
                    ((Card) cards[i]).customer = this;
                }
                if (subscription != null) {
                    subscription.customer = this;
                }
            }
            catch (Exception ex)
            {
                throw new Error(ex.ToString());
            }
        }

        public static Customer find(String id) {
            try{
                //String className = Customer.class.getCanonicalName();
                return (Customer) scpFind("customer", id);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static Customer create(JObject _params){
            try { 
                //String className = Customer.class.getCanonicalName();
                return (Customer) scpCreate("customer", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where(JObject _params) {
            try { 
                //String className = Customer.class.getCanonicalName();
                return (ConektaObject) scpWhere("customer", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where() {
            try { 
                //String className = Customer.class.getCanonicalName();
                return (ConektaObject) scpWhere("customer", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void delete() {
            try { 
                this.delete(null, null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void update(JObject _params) {
            try { 
                base.update(_params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public Card createCard(JObject _params){
            return (Card) this.createMember("cards", _params);
        }

        public Subscription createSubscription(JObject _params) {
            return (Subscription) this.createMember("subscription", _params);
        }
    }
}
