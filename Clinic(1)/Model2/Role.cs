using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class Role
{
    public int IdRoles { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Emplo> Emplos { get; set; } = new List<Emplo>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
