using System;
using System.Collections.Generic;

namespace H4ServersideProgrammering.Models;

public partial class Todolist
{
    public int Id { get; set; }

    public string User { get; set; } = null!;

    public string Item { get; set; } = null!;
}
