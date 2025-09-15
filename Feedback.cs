using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using آخرین_الماس.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace آخرین_الماس.Models
{    [Table("Feedbacks")]
    public class Feedback
    {
        public int Id { get; set; }
        public string Username { get; set; }  // نام کاربری
        public string Message { get; set; }   // متن نظر
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
     }

