using System;
using Newtonsoft.Json.Linq;

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
            return (ConektaObject) scpWhere("event", _params);
        }

        public static ConektaObject where() {
            return (ConektaObject) scpWhere("event", null);
        }
    }
}
