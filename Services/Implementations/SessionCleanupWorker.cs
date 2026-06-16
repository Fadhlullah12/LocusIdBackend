using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;

public class SessionCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SessionCleanupWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var sessionRepository = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            var attendanceRepository = scope.ServiceProvider.GetRequiredService<IAttendanceRepository>();
            var courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();

            // 1. Identify expired sessions
            // LOGIC FIX: Check against the calculated EndTime or (CreatedAt + Duration)
            var expiredSessions = await sessionRepository.GetAll(s => 
                s.IsActive && s.CreatedAt.AddMinutes(s.Duration) <= DateTime.Now);

            if (expiredSessions.Any())
            {
                foreach (var session in expiredSessions)
                {
                    // 1. Fetch the course with its joiner table
                    var course = await courseRepository.Get(c => c.Id == session.CourseId);
                    
                    if (course?.CourseStudents == null) continue;

                    // 2. Get the list of Student IDs from the joiner table
                    var enrolledStudentIds = course.CourseStudents
                        .Select(cs => cs.StudentId)
                        .ToList();

                    // 3. Get the list of Student IDs who DID show up (already have an Attendance record)
                    // Note: Make sure session.Attendances is loaded (Include)
                    var presentStudentIds = session.Attendances
                        .Select(a => a.StudentId)
                        .ToList();

                    // 4. Find the students who are in the course but NOT in the attendance list
                    var absentStudentIds = enrolledStudentIds.Except(presentStudentIds).ToList();

                    // 5. Create the "Absent" records
                    foreach (var studentId in absentStudentIds)
                    {
                        var absenceRecord = new Attendance
                        {
                            SessionId = session.Id,
                            StudentId = studentId,
                            Attended = false, 
                            CreatedAt = DateTime.Now,
                        };
                        await attendanceRepository.Create(absenceRecord);
                    }

                    // 6. Finalize the session
                    session.IsActive = false;
                    session.EndTime = DateTime.Now;
                }

                await sessionRepository.Save();
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
    }
}
}