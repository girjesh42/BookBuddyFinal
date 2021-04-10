using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BookBuddyFinal.Models
{
    public partial class UsersAddress
    {
        public UsersAddress()
        {
            Cart = new HashSet<Cart>();
            Order = new HashSet<Order>();
            PlacedOrder = new HashSet<PlacedOrder>();
        }

        public int UserAddressId { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public int UserPostalCode { get; set; }
        public string UserCountry { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string UserPhone1 { get; set; }
        public string UserPhone2 { get; set; }
        public string UserEmail1 { get; set; }
        public string UserEmail2 { get; set; }
        public string UserCo { get; set; }
        public string UserAddress1 { get; set; }
        public string UserAddress2 { get; set; }
        public string UserAddress3 { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<Cart> Cart { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<PlacedOrder> PlacedOrder { get; set; }
    }
}
