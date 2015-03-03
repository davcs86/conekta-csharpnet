using System;

namespace ConektaCSharp
{
    public class LineItems:Resource
    {
        public String name;
        public String sku;
        public int unit_price;
        public String description;
        public int quantity;
        public String type;
        public String category;

        public LineItems(String id = null) : base(id) { }
        public LineItems() : base() { }
    }
}
