﻿using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Poll
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Tekst { get; set; } = null!;

    public DateTime? PollDate { get; set; }

    public int? KolegijId { get; set; }

    public int? StudijId { get; set; }

    public int? GodinaId { get; set; }

    public virtual Godina? Godina { get; set; }

    public virtual Kolegij? Kolegij { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual Studij? Studij { get; set; }
}
