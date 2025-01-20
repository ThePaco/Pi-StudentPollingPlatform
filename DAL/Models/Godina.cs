using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Godina
{
    public int Idgodina { get; set; }

    public int? BrojGodine { get; set; }

    public virtual ICollection<Poll> Polls { get; set; } = new List<Poll>();
}
