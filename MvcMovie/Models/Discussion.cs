using System;
using System.Collections.Generic;

namespace Diskussion.Models;

public partial class Discussion
{
    public long Id { get; set; }

    public long IdAuthor { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}
