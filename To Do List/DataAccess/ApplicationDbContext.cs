using Microsoft.EntityFrameworkCore;
using System.Net;
using To_Do_List.Models;

namespace To_Do_List.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoList> toDoLists { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;initial catalog=ToDoList;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;");
        }
    }
}
