using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Billing
{
    public int BillingId { get; set; }

    public int? PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Insurance { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Patient? Patient { get; set; }
}
