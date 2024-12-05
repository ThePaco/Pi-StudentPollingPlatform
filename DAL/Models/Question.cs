using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Tekst { get; set; } = null!;

    public int PollId { get; set; }

    public virtual Poll Poll { get; set; } = null!;

    public virtual ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();
}
