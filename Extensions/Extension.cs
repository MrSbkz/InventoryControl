using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace InventoryControl.Extensions;

public static class Extension
{
    public static string GetAttribute(this Enum enumValue)
    {
        var displayName = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName();
        if (String.IsNullOrEmpty(displayName))
        {
            displayName = enumValue.ToString();
        }
        return displayName;
    }
}