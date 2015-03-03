using System;

namespace ConektaCSharp
{
    public class Shipment:Resource
    {
        public String carrier;
        public String service;
        public String tracking_id;
        public int price;
        public Address address;
        
        public Shipment(String id = null) : base(id) { }
        public Shipment() : base() { }
    }
}
