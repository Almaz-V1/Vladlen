using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Specialist
{
    public int IdSpecialist { get; set; }

    public string TitleSpecialist { get; set; } = null!;

    public virtual ICollection<Emplo> Emplos { get; set; } = new List<Emplo>();
}
