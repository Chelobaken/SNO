using System;
using System.Collections.Generic;

namespace snoapi;

public partial class Project
{
    public int Projectid { get; set; }

    public string Title { get; set; } = null!;

    public string Mddescription { get; set; } = null!;
}
