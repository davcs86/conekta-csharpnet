using System;

namespace ConektaCSharp
{
    public class Details : Resource
    {
        public String name;
        public String phone;
        public String email;
        public String date_of_birth;
        public ConektaObject line_items;
        public Address billing_address;
        public Shipment shipment;

        public Details(String id = null) : base(id) { }
        public Details() : base() { }
    }
}
