using System;
using System.Collections.Generic;

#nullable disable

namespace I_Dilmac_Asp_net_core_5._0_api.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
