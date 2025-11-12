using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models
{
    public class ToDoItem
    {
        [Key]
        [Display(Name = "ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool isDone { get; set; }
    }
}
