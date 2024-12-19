using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Kolegij
{
    public int Idkolegij { get; set; }

    public string? KolegijName { get; set; }

    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();
}
