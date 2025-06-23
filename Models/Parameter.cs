using System;
using System.Collections.Generic;

namespace ServerApiClient.Models;

public partial class Parameter
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ServerParameter> ServerParameters { get; set; } = new List<ServerParameter>();
}
