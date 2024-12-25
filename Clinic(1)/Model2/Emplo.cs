using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Emplo
{
    public int IdDoctors { get; set; }

    public string Name { get; set; } = null!;

    public string LasteName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Emaile { get; set; } = null!;

    public string? CabinetNumber { get; set; }

    public byte[]? Image { get; set; }

    public int NameIdRoles { get; set; }

    public string Password { get; set; } = null!;

    public int IdProfeshens { get; set; }

    public string NomerPasport { get; set; } = null!;

    public string SeriaPasport { get; set; } = null!;

    public DateOnly Age { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Specialist IdProfeshensNavigation { get; set; } = null!;

    public virtual Role NameIdRolesNavigation { get; set; } = null!;
}
