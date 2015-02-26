using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConektaCSharp
{
    public class Refunds:Resource
    {
        public int created_at;
        public int amount;
        public String currency;
        public String transaction;
        public String auth_code;
        
        public Refunds(String id = null) : base(id) { }
        public Refunds() : base() { }
    }
}
