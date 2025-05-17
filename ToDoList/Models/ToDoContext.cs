﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
            : base(options) { }

        public DbSet<ToDoItem> ToDoItems { get; set; }
    }

}
