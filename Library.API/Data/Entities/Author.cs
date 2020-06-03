using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Data.Entities
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        public ICollection<Book> Books { get; set; }
            = new List<Book>();

        [ForeignKey("UserId")]
        public LibraryUser LibraryUser { get; set; }
        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
