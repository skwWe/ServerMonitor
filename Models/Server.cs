using System;
using System.Collections.Generic;

namespace ServerApiClient.Models;

public partial class Server
{
    public Guid Id { get; set; }

    public string HostName { get; set; } = null!;

    public string IpAddres { get; set; } = null!;

    public Guid BlockId { get; set; }

    public bool State { get; set; }

    public virtual Block? Block { get; set; } = null!;

    public virtual ICollection<Error> Errors { get; set; } = new List<Error>();

    public virtual ICollection<Metric> Metrics { get; set; } = new List<Metric>();

    public virtual ICollection<ServerParameter> ServerParameters { get; set; } = new List<ServerParameter>();
}
