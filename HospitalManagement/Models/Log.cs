using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Log
{
    public int LogsId { get; set; }

    public string? Action { get; set; }

    public string? PerformedBy { get; set; }

    public DateTime? PerformedAt { get; set; }
}
