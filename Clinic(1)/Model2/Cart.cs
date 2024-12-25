using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Cart
{
    public int IdCart { get; set; }

    public int IdEmpoes { get; set; }

    public DateTime DateTime { get; set; }

    public int NomerCabin { get; set; }

    public string LasteName { get; set; } = null!;

    public string? Patronomick { get; set; }

    public string Name { get; set; } = null!;

    public string Spec { get; set; } = null!;

    public virtual ICollection<Chek> Cheks { get; set; } = new List<Chek>();

    public virtual Emplo IdEmpoesNavigation { get; set; } = null!;
}
