using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Students_Information.Models
{
    public enum Gender { boy = 1, girl }
    public class Student
    {
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        public string StudentName { get; set; } = default!;
        [Required]
        public int Age { get; set; }
        [Required, EnumDataType(typeof(Gender))]
        public Gender? Gender { get; set; }
        [Required, StringLength(50)]
        public string? Picture { get; set; } = default!;
        public bool? IsRegular { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
    public class Course
    {
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        public string CourseName { get; set; } = default!;

        [Required, Column(TypeName = "money")]
        public decimal CourseFee { get; set; } = default!;
        [Required, Column(TypeName = "date")]
        public DateTime? AdmissionDate { get; set; }
        [Required, ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Random random = new();
            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<Student>().HasData(
                    new Student { StudentId = i, StudentName = "Student " + i, Age = random.Next(10, 30), Gender = (Gender)random.Next(1, 5), Picture = i + "jpg" }
                    );
            }
            for (int i = 1; i <= 8; i++)
            {
                modelBuilder.Entity<Course>().HasData(
                    new Course { CourseId = i,CourseName="Course"+ i ,CourseFee = random.Next(400, 500) * i, AdmissionDate = DateTime.Now.AddDays(-1 * random.Next(400, 500)), StudentId = (i % 5 == 0 ? 5 : i % 5) }
                    );
            }
        }
    }
}
