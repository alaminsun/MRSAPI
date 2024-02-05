namespace MRSAPI.Models
{
    public class FileUploadDTO
    {
        public int DoctorId { get; set; }
        public List<IFormFile> Files { get; set; }

        public List<string> AttachmentTypes { get; set; }
        //public List<FileUploadModel> FileUploadInfoList { get; set; }

        //public List<FileInfo> FileInfos { get; set; }
    }
    public class FileUploadModel
    {
        //public List<string> AttachmentTypes { get; set; }
        public long Id { get; set; }
        public int DoctorId { get; set; }
        //public IFormFile File { get; set; }
        public List<IFormFile> Files { get; set; }
        //////public List<FileInfo> FileInfos { get; set; }
        ////public List<string> FilePathList { get; set; }
        public List<string> AttachmentTypes { get; set; }
        public List<string> FilePathList { get; set; }
        //public string AttachmentTypes { get; set; }

    }

    //public class FileInfo
    //{
    //    public IFormFile File { get; set; }
    //    public string FilePathList { get; set; }
    //    public string AttachmentTypes { get; set; }
    //}
}
