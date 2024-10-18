using System;
using System.Collections.Generic;

namespace SNO.API;

public partial class User
{
    public int Userid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Usernamevk { get; set; }

    public string? Email { get; set; }

    public string? Usernametg { get; set; }

    public string? Phonenumber { get; set; }
}
