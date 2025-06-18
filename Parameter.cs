using System;
using System.Collections.Generic;

namespace ServerApiClient;

public partial class Parameter
{
    public Guid RequestId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid IdServer { get; set; }

    public int RamMb { get; set; }

    public int CpuPercent { get; set; }

    public int RomMb { get; set; }

    public virtual Server IdServerNavigation { get; set; } = null!;
}
