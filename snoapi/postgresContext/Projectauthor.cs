using System;
using System.Collections.Generic;

namespace SNO.API;

public partial class Projectauthor
{
    public int Userid { get; set; }

    public int Projectid { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
