using System;
using System.Collections.Generic;

namespace ServerApiClient.Models;

public partial class Block
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();
}
