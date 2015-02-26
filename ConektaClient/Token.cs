using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConektaCSharp
{
    public class Token : Resource
    {
        public Boolean livemode;
        public Boolean used;

        public Token(String _id = null) : base(_id) { }

        public Token() : base() { }

        public static Token find(String _id)
        {
            try { 
                //String className = Token.class.getCanonicalName();
                return (Token)scpFind("token", _id);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }

        public static Token create(JObject _params)
        {
            try { 
                //String className = Token.class.getCanonicalName();
                return (Token)scpCreate("token", _params);
            }
            catch (Exception e)
            {
                throw new Error(e.ToString());
            }
        }
    }
}
