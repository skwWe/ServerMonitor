using System;
using System.Collections.Generic;

namespace ServerApiClient.Models;

public partial class Error
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int Importance { get; set; }

    public bool State { get; set; }

    public Guid ServerId { get; set; }

    public string Message { get; set; } = null!;

    public virtual Server Server { get; set; } = null!;
}
