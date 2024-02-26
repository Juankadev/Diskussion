using System.ComponentModel.DataAnnotations;

namespace Diskussion.Models.ViewModels
{
    public class DiscussionViewModel
    {
        [Required(ErrorMessage = "El campo Titulo es obligatorio")]
        public string Title { get; set; }

        public string? Description { get; set; }
    }
}
