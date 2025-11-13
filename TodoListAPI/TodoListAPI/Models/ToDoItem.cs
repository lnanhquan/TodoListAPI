using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
