using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace آخرین_الماس.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int FoodId { get; set; }

        [Column("FoodName")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int UserId { get; set; } // چون Identity معمولاً UserId رو string نگه می‌داره

        [NotMapped]
        public int TotalPrice => Quantity * Price;
    }
}

