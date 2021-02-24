using System;
using System.Collections.Generic;

#nullable disable

namespace I_Dilmac_Asp_net_core_5._0_api.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
