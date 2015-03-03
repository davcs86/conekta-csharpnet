using System;

namespace ConektaCSharp
{
    public class Refunds : Resource
    {
        public int amount;
        public String auth_code;
        public int created_at;
        public String currency;
        public String transaction;

        public Refunds(String id = null) : base(id)
        {
        }

        public Refunds()
        {
        }
    }
}