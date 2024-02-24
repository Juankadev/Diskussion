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

    public bool? State { get; set; }

    public virtual User IdAuthorNavigation { get; set; } = null!;

    public virtual Discussion IdDiscussionNavigation { get; set; } = null!;
}
