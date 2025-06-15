using System;
using System.Collections.Generic;

namespace ServerApiClient;

public partial class Problem
{
    public Guid IdProblem { get; set; }

    public DateTime DateTimeProblem { get; set; }

    public DateTime? DateProblemSolution { get; set; }

    public Guid IdErrorImportance { get; set; }

    public bool StatusProblem { get; set; }

    public Guid IdServer { get; set; }

    public string MessageProblem { get; set; } = null!;

    public virtual ErrorImportance IdErrorImportanceNavigation { get; set; } = null!;

    public virtual Server IdServerNavigation { get; set; } = null!;
}
