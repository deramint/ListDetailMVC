using System.ComponentModel.DataAnnotations;

namespace ListDetailMVC.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Display(Name = "著者")]
        public string Name { get; set; }
        public int Age { get; set; }
        public int PerfectureId { get; set; }

        public virtual ICollection<Book>? Book { get; set; }
        public virtual Perfecture? Perfecture { get; set; }
    }
}
