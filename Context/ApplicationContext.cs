using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<LecturerCourse> LecturerCourses { get; set; }
        public DbSet<StudentSession> StudentSessions { get; set; }
        public DbSet<AcademicSession> AcademicSessions { get; set; }
        public DbSet<DepartmentCourse> DepartmentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student-User relationship (one-to-one)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.User)
                .WithOne(s => s.Student)
                .HasForeignKey<Student>("UserId")
                .IsRequired(true);

            // Configure Lecturer-User relationship (one-to-one)
            modelBuilder.Entity<Lecturer>()
                .HasOne(u => u.User)
                .WithOne(l => l.Lecturer)
                .HasForeignKey<Lecturer>("UserId")
                .IsRequired(true);

            // Configure Student-Attendance relationship (one-to-many)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId);

            // Configure Session-Attendance relationship (one-to-many)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Session)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.SessionId);

            // Configure Course-Session relationship (one-to-many)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sessions)
                .HasForeignKey(s => s.CourseId);

            // Configure Lecturer-Session relationship (one-to-many)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Lecturer)
                .WithMany(l => l.Sessions)
                .HasForeignKey(s => s.LecturerId);

            // Configure StudentCourse relationship (many-to-many junction)
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(sc => sc.CourseId);

            // Configure LecturerCourse relationship (many-to-many junction)

            // Configure StudentSession relationship (many-to-many junction)
            modelBuilder.Entity<StudentSession>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSessions)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder.Entity<StudentSession>()
                .HasOne(ss => ss.Session)
                .WithMany(s => s.StudentSessions)
                .HasForeignKey(ss => ss.SessionId);
        }
    }
}
