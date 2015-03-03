using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Event : Resource
    {
        public int created_at;
        public Boolean livemode;
        public String type;

        public Event(String _id = null) : base(_id)
        {
        }

        public Event()
        {
        }

        public static ConektaObject where(JObject _params)
        {
            return scpWhere("event", _params);
        }

        public static ConektaObject where()
        {
            return scpWhere("event", null);
        }
    }
}