﻿using Library.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Services
{
    public interface IBookRepository : IBaseRepository<Book>
    {
    }
}