namespace InventoryControl.Helper
{
    public static class HttpContextHelper
    {
        public static IList<string> GetRoleFromContext(HttpContext context)
        {
            return context.User.Claims.Where(x => x.Type.Contains("role")).Select(x => x.Value).ToList();
        }

        public static string? GetUserFromContext(HttpContext context)
        {
            return context.User.Identity?.Name;
        }
    }
}