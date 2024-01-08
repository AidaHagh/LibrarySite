using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public int BookPageCount { get; set; }
        public string? BookImage { get; set; }
        public int Price { get; set; }
        public int BookStock { get; set; }
        public int BookViews { get; set; }
        public int BookLikeCount { get; set; }

        ///////////////////////////forigen Key///////////////
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }


        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
