using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Card : Resource
    {
        public Customer customer;
        public Boolean deleted;
        public String exp_month;
        public String exp_year;
        public String last4;
        public String name;

        public Card(String id = null) : base(id)
        {
        }

        public Card()
        {
        }

        public override String instanceUrl()
        {
            if (id.Length == 0)
            {
                throw new Error("Could not get the id of Resource instance.");
            }
            var _base = customer.instanceUrl();
            return _base + "/cards/" + id;
        }

        public void update(JObject _params)
        {
            base.update(_params);
        }

        public void delete()
        {
            delete("customer", "cards");
        }
    }
}