using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Prescription
{
    public int PrescriptionsId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public int? MedicationId { get; set; }

    public string? Dosage { get; set; }

    public string? Instructions { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual Medication? Medication { get; set; }

    public virtual Patient? Patient { get; set; }
}
