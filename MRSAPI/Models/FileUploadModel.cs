namespace MRSAPI.Models
{
    public class FileUploadDTO
    {
        public int DoctorId { get; set; }
        public List<IFormFile> Files { get; set; }

        public string? AttachmentType { get; set; }
    }
    public class FileUploadModel
    {
        public long Id { get; set; }
        public int DoctorId { get; set; }
        //public IFormFile File { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<string> FilePathList { get; set; }
        public string? AttachmentType { get; set; }
    }
}
