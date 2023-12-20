namespace MRSAPI.Models
{
    public class DoctorAttachmentModel
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string FileType { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }
}
