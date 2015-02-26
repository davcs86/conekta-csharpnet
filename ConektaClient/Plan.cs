using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConektaCSharp
{
    public class Plan:Resource
    {
        public Boolean livemode;
        public Boolean deleted;
        public int created_at;
        public String name;
        public String interval;
        public int frequency;
        public int interval_total_count;
        public int trial_period_days;
        public String currency;
        public int amount;

        public Plan(String _id=null):base(_id){}

        public Plan():base(){}

        public static Plan find(String _id)  {
            try { 
                //String className = Plan.class.getCanonicalName();
                return (Plan) scpFind("plan", _id);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where(JObject _params) {
            try
            {
                //String className = Plan.class.getCanonicalName();
                return (ConektaObject)scpWhere("plan", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where() {
            try { 
                //String className = Plan.class.getCanonicalName();
                return (ConektaObject) scpWhere("plan", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static Plan create(JObject _params) {
            try { 
                //String className = Plan.class.getCanonicalName();
                return (Plan) scpCreate("plan", _params);
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
    }
}
