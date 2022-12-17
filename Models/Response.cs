namespace InventoryControl.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }

        public T? Data { get; set; }

        public IList<string>? Errors { get; set; }
    }
}
