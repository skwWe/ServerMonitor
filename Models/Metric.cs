using System;
using System.Collections.Generic;

namespace ServerApiClient.Models;

public partial class Metric
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid ServerId { get; set; }

    public int Ram { get; set; }

    public int Cpu { get; set; }

    public int Strorage { get; set; }

    public virtual Server Server { get; set; } = null!;
}
