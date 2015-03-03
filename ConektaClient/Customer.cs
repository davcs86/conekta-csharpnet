using System;
using Newtonsoft.Json.Linq;

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

        public static Customer find(String id) {
            return (Customer) scpFind("customer", id);
        }

        public static Customer create(JObject _params){
            return (Customer) scpCreate("customer", _params);
        }

        public static ConektaObject where(JObject _params) {
            return (ConektaObject) scpWhere("customer", _params);
        }

        public static ConektaObject where() {
            return (ConektaObject) scpWhere("customer", null);
        }

        public void delete() {
            this.delete(null, null);
        }

        public void update(JObject _params) {
            base.update(_params);
        }

        public Card createCard(JObject _params){
            return (Card) this.createMember("cards", _params);
        }

        public Subscription createSubscription(JObject _params) {
            return (Subscription) this.createMember("subscription", _params);
        }
    }
}
