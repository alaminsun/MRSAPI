namespace MRSAPI.Models
{
    public class FileUploadModel
    {
        public int DoctorId { get; set; }
        public IFormFile File { get; set; }
        //public FileType FileType { get; set; }
        //public string? FilePath { get; set; }
        //public string? FileName { get; set; }
        public string? AttachmentType { get; set; }
    }
}
