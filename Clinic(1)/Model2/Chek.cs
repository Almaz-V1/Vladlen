using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Chek
{
    public int Idchek { get; set; }

    public string Lastename { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronomick { get; set; }

    public int NomerCabi { get; set; }

    public string Profecsens { get; set; } = null!;

    public DateTime Date { get; set; }

    public int IdCart { get; set; }

    public virtual Cart IdCartNavigation { get; set; } = null!;
}
