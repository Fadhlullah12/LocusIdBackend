namespace LocusIDBackend.Dtos.ResponseModels
{
    public class AcademicSessionDto
    {
         public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}