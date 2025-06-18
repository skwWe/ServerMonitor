using System;
using System.Collections.Generic;

namespace ServerApiClient;

public partial class Server
{
    public Guid IdServer { get; set; }

    public string NameServer { get; set; } = null!;

    public string IpAdress { get; set; } = null!;

    public Guid IdServerGroup { get; set; }

    public bool? ServerStatus { get; set; }

    public virtual ServersGroup IdServerGroupNavigation { get; set; } = null!;

    public virtual ICollection<Parameter> Parameters { get; set; } = new List<Parameter>();

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();
}
