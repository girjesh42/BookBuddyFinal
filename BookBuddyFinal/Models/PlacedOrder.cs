using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BookBuddyFinal.Models
{
    public partial class PlacedOrder
    {
        public int Id { get; set; }
        public int? CartId { get; set; }
        public int? AddressId { get; set; }
        public string UserId { get; set; }

        public virtual UsersAddress Address { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
