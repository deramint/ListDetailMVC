using System.ComponentModel.DataAnnotations;

namespace ListDetailMVC.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Display(Name = "出版社")]
        public string Title { get; set; }
        public virtual ICollection<Book>? Book { get; set; }
    }
}
