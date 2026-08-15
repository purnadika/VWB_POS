using System.Collections.Generic;

namespace POS.Domain.Entities;

public class Employee : Person
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public List<string> GrantedModules { get; set; } = new();

    public bool HasPermission(string moduleName)
    {
        return GrantedModules.Contains(moduleName);
    }
}

