using System;
using System.Collections.Generic;

namespace HospitalManagement.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string? StaffName { get; set; }

    public string? StaffRole { get; set; }

    public string? Contact { get; set; }

    public string? StaffAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
