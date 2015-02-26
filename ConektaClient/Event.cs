using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConektaCSharp
{
    public class Event:Resource
    {
        public Boolean livemode;
        public int created_at;
        public String type;

        public Event(String _id=null):base(_id){}

        public Event() :base(){}

        public static ConektaObject where(JObject _params) {
            try { 
                //String className = Event.class.getCanonicalName();
                return (ConektaObject) scpWhere("event", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static ConektaObject where() {
            try { 
                //String className = Event.class.getCanonicalName();
                return (ConektaObject) scpWhere("event", null);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
