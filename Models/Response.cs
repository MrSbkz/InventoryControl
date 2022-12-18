namespace InventoryControl.Models
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; } = false;

        public T? Data { get; set; }

        public IList<string>? Errors { get; set; }
    }
}
