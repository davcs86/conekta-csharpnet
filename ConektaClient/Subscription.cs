using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            try { 
                base.update(_params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void pause() {
            try { 
                this.customAction("POST", "pause", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void cancel() {
            try { 
                this.customAction("POST", "cancel", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void resume() {
            try { 
                this.customAction("POST", "resume", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
