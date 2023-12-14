using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRSAPI.Models
{
    public class FileDetails
    {
        
        
        public string FileName { get; set; }
        //public byte[] FileData { get; set; }
        public string FilePath { get; set; }
        public FileType FileType { get; set; }
    }
}
