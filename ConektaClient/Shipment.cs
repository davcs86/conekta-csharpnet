using System;

namespace ConektaCSharp
{
    public class Shipment : Resource
    {
        public Address address;
        public String carrier;
        public int price;
        public String service;
        public String tracking_id;

        public Shipment(String id = null) : base(id)
        {
        }

        public Shipment()
        {
        }
    }
}