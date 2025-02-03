using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int? PatientId { get; set; }

    public string? ReportType { get; set; }

    public string? ReportDetails { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Patient? Patient { get; set; }
}
