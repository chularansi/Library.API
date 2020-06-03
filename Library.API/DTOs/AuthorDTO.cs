﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }

        public virtual IList<BookDTO> Books { get; set; } = new List<BookDTO>();
    }
}
