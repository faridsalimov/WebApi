using Microsoft.EntityFrameworkCore;
using WebApiTask1.Entities;

namespace WebApiTask1.Data
{
    public class StudentDbContext:DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            :base(options) 
        {
            
        }

        public DbSet<Student> Students { get; set; }    

    }
}
