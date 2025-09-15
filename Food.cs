using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using آخرین_الماس.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace آخرین_الماس.Models
    {
        public class Food
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
             public string ImagePath { get; set; }

    }
}

