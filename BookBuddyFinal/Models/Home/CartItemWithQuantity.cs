using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBuddyFinal.Models.Home
{
    public class CartItemWithQuantity
    {
        public Products products { get; set; }
        public int Quantity { get; set; }
    }
}
