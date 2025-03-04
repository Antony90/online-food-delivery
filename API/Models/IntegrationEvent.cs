using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public abstract class IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;
}