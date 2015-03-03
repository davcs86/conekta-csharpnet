using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Subscription : Resource
    {
        public int billing_cycle_end;
        public int billing_cycle_start;
        public String card_id;
        public int created_at;
        public Customer customer;
        public String plan_id;
        public String status;

        public Subscription(String id = null) : base(id)
        {
        }

        public Subscription()
        {
        }

        public override String instanceUrl()
        {
            if (id.Length == 0)
            {
                throw new Error("Could not get the id of Resource instance.");
            }
            var _base = customer.instanceUrl();
            return _base + "/subscription";
        }

        public void update(JObject _params)
        {
            base.update(_params);
        }

        public void pause()
        {
            customAction("POST", "pause", null);
        }

        public void cancel()
        {
            customAction("POST", "cancel", null);
        }

        public void resume()
        {
            customAction("POST", "resume", null);
        }
    }
}