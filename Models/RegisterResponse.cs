namespace InventoryControl.Models
{
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }

        public IList<string>? Data { get; set; }
    }
}
