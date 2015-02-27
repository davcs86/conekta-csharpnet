using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Card:Resource
    {
        public Customer customer;
        public String name;
        public String last4;
        public String exp_month;
        public String exp_year;
        public Boolean deleted;

        public Card(String id = null) : base(id) { }
        public Card() : base() { }

        public override String instanceUrl() {
            try
            {
                if (id.Length == 0)
                {
                    throw new Error("Could not get the id of Resource instance.");
                }
                String _base = customer.instanceUrl();
                return _base + "/cards/" + id;
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void update(JObject _params){
            try{
                base.update(_params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public void delete(){
            try {
                this.delete("customer", "cards");
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
