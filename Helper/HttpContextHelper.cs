namespace InventoryControl.Helper
{
    public class HttpContextHelper
    {
        public static IList<string> GetRoleFromContext(HttpContext context) 
        {
            return context.User.Claims.Where(x=>x.Type.Contains("role")).Select(x=>x.Value).ToList();
        }
    }
}
