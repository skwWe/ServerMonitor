using System;
using System.Collections.Generic;

namespace ServerApiClient;

public partial class ServersGroup
{
    public Guid IdServerGroup { get; set; }

    public string NameServerGroup { get; set; } = null!;

    public virtual ICollection<Server> Servers { get; set; } = new List<Server>();
}
