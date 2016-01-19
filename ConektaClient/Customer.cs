using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Customer : Resource
    {
        public ConektaObject cards;
        public int created_at;
        public String default_card_id;
        public Boolean deleted;
        public String email;
        public Boolean livemode;
        public String name;
        public String phone;
        public Subscription subscription;

        public Customer(String id = null) : base(id)
        {
            cards = new ConektaObject();
            subscription = new Subscription();
        }

        public Customer()
        {
            cards = new ConektaObject();
            subscription = new Subscription();
        }

        public override void LoadFromObject(JObject jsonObject)
        {
            if (jsonObject != null)
            {
                try
                {
                    base.LoadFromObject(jsonObject);
                }
                catch (Exception e)
                {
                    throw new Error(e.Message);
                }
            }
            foreach (Card c in cards)
            {
                c.customer = this;
            }
            if (subscription != null)
            {
                subscription.customer = this;
            }
        }

        public static Customer find(String id)
        {
            return (Customer) scpFind("customer", id);
        }

        public static Customer create(JObject _params)
        {
            return (Customer) scpCreate("customer", _params);
        }

        public static ConektaObject where(JObject _params)
        {
            return scpWhere("customer", _params);
        }

        public static ConektaObject where()
        {
            return scpWhere("customer", null);
        }

        public void delete()
        {
            delete(null, null);
        }

        public new void update(JObject _params)
        {
            base.update(_params);
        }

        public Card createCard(JObject _params)
        {
            return (Card) createMember("cards", _params);
        }

        public Subscription createSubscription(JObject _params)
        {
            return (Subscription) createMember("subscription", _params);
        }
    }
}