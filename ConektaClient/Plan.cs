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

        public Plan(String id=null):base(id){}

        public Plan():base(){}

        public static Plan find(String id)  {
            return (Plan) scpFind("plan", id);
        }

        public static ConektaObject where(JObject _params) {
            return (ConektaObject)scpWhere("plan", _params);
        }

        public static ConektaObject where() {
            return (ConektaObject) scpWhere("plan", null);
        }

        public static Plan create(JObject _params) {
            return (Plan) scpCreate("plan", _params);
        }

        public void update(JObject _params)
        {
            base.update(_params);
        }

        public void delete() {
            this.delete(null, null);
        }
    }
}
