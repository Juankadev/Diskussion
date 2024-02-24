using System;
using System.Collections.Generic;

namespace Diskussion.Models;

public partial class Response
{
    public long Id { get; set; }

    public long IdAuthor { get; set; }

    public long IdDiscussion { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Discussion Discussion { get; set; } = null!;
}
