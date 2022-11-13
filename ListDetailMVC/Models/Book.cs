using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace ListDetailMVC.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Display(Name = "書籍名")]
        public string Title { get; set; }

        public int AuthorId { get; set; }
        public int PublisherId { get; set; }

        [Display(Name = "価格")]
        public int price { get; set; }

        public virtual Author? Author { get; set; }
        public virtual Publisher? Publisher { get; set; }

        [Display(Name = "発売日")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "ISBNコード")]
        [RegularExpression("[0-9]{3}-[0-9]{1}-[0-9]{3,5}-[0-9]{3,5}-[0-9A-Z]{1}")]
        public string ISBN { get; set; }

    }
}
