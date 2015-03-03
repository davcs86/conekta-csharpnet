using System;

namespace ConektaCSharp
{
    public class Address:Resource
    {
        public String street1;
        public String street2;
        public String street3;
        public String city;
        public String state;
        public String zip;
        public String country;
        public String tax_id;
        public String company_name;
        public String phone;
        public String email;

        public Address(String id = null) : base(id) { }
        public Address() : base() { }
    }
}
