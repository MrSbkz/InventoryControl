namespace InventoryControl.Models
{
    public class RegisterRespons
    {
        public bool IsSuccess { get; set; }

        public IList<string>? Data { get; set; }
    }
}
