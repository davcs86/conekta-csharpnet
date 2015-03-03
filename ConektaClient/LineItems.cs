using System;

namespace ConektaCSharp
{
    public class LineItems : Resource
    {
        public String category;
        public String description;
        public String name;
        public int quantity;
        public String sku;
        public String type;
        public int unit_price;

        public LineItems(String id = null) : base(id)
        {
        }

        public LineItems()
        {
        }
    }
}