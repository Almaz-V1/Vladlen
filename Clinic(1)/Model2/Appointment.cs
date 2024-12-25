using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Appointment
{
    public int IdAppointment { get; set; }

    public DateTime Date { get; set; }

    public string LasteNames { get; set; } = null!;

    public string Names { get; set; } = null!;

    public string? Patronomicl { get; set; }

    public int NomeKabi { get; set; }

    public string Spec { get; set; } = null!;

    public int IdCart { get; set; }
}
