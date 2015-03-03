using System;

namespace ConektaCSharp
{
    public class Details : Resource
    {
        public Address billing_address;
        public String date_of_birth;
        public String email;
        public ConektaObject line_items;
        public String name;
        public String phone;
        public Shipment shipment;

        public Details(String id = null) : base(id)
        {
        }

        public Details()
        {
        }
    }
}