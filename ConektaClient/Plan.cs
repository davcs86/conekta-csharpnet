using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Plan : Resource
    {
        public int amount;
        public int created_at;
        public String currency;
        public Boolean deleted;
        public int frequency;
        public String interval;
        public int interval_total_count;
        public Boolean livemode;
        public String name;
        public int trial_period_days;

        public Plan(String id = null) : base(id)
        {
        }

        public Plan()
        {
        }

        public static Plan find(String id)
        {
            return (Plan) scpFind("plan", id);
        }

        public static ConektaObject where(JObject _params)
        {
            return scpWhere("plan", _params);
        }

        public static ConektaObject where()
        {
            return scpWhere("plan", null);
        }

        public static Plan create(JObject _params)
        {
            return (Plan) scpCreate("plan", _params);
        }

        public void update(JObject _params)
        {
            base.update(_params);
        }

        public void delete()
        {
            delete(null, null);
        }
    }
}