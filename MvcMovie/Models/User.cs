using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Diskussion.Models;

public partial class User
{
    public long Id { get; set; }

    [Required(ErrorMessage ="El campo Nombre es obligatorio")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage ="El campo Email es obligatorio")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage ="El campo Contraseña es obligatorio")]
    public string Password { get; set; } = null!;

    public DateTime? RegistrationDate { get; set; }

    public bool? State { get; set; }

    public virtual ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
