using System;

namespace ConektaCSharp
{
    public class Address : Resource
    {
        public String city;
        public String company_name;
        public String country;
        public String email;
        public String phone;
        public String state;
        public String street1;
        public String street2;
        public String street3;
        public String tax_id;
        public String zip;

        public Address(String id = null) : base(id)
        {
        }

        public Address()
        {
        }
    }
}