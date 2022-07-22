namespace NlsDemo.data.Entity
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
