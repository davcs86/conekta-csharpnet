using System;
using Newtonsoft.Json.Linq;

namespace ConektaCSharp
{
    public class Token : Resource
    {
        public Boolean livemode;
        public Boolean used;

        public Token(String id = null) : base(id) { }

        public Token() : base() { }

        public static Token find(String id)
        {
            return (Token)scpFind("token", id);
        }

        public static Token create(JObject _params)
        {
            return (Token)scpCreate("token", _params);
        }
    }
}
