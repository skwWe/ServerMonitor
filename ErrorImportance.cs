using System;
using System.Collections.Generic;

namespace ServerApiClient;

public partial class ErrorImportance
{
    public Guid IdErrorImportance { get; set; }

    public string NameErrorImportances { get; set; } = null!;

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();
}
