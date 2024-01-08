using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class BorrowBook
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }

        //1-درخواست امانت
        //2 - اگر ادمین تایید کرد
        //3 - اگر کتاب را پس آورد
        public byte Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? AnswereDate { get; set; }
        public DateTime? GiveBackDate { get; set; }
        public int Price { get; set; }
    }
}
