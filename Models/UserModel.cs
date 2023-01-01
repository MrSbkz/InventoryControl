﻿namespace InventoryControl.Models;

public class UserModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}