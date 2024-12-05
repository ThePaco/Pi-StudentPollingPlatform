using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class UserAnswer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuestionId { get; set; }

    public int Answer { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
