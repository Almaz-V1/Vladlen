using System;
using System.Collections.Generic;

namespace Clinic_1_.Model2;

public partial class User
{
    public int IdUsers { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Partominyc { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordNumber { get; set; } = null!;

    public int IdRole { get; set; }

    public string PasswordSeries { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly Age { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
