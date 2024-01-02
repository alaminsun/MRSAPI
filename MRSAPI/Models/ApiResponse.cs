namespace MRSAPI.Models
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public long DoctorId { get; set; }
        public T Data { get; set; }
    }
}
