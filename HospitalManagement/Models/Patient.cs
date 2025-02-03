using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string? PatientName { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Contact { get; set; }

    public string? PatientAddress { get; set; }

    public string? MedicalHistory { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
