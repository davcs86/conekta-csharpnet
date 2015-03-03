using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Subscription:Resource
    {
        public Customer customer;
        public String status;
        public int created_at;
        public int billing_cycle_start;
        public int billing_cycle_end;
        public String plan_id;
        public String card_id;
        
        public Subscription(String id = null) : base(id) { }
        public Subscription() : base() { }

        public override String instanceUrl() {
            try { 
                if (id.Length == 0) {
                    throw new Error("Could not get the id of Resource instance.");
                }
                String _base = this.customer.instanceUrl();
                return _base + "/subscription";
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void update(JObject _params) {
            base.update(_params);
        }

        public void pause() {
            this.customAction("POST", "pause", null);
        }

        public void cancel() {
            this.customAction("POST", "cancel", null);
        }

        public void resume() {
            this.customAction("POST", "resume", null);
        }
    }
}
