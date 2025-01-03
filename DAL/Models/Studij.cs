using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Studij
{
    public int Idstudij { get; set; }

    public string? StudijName { get; set; }

    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();
}
