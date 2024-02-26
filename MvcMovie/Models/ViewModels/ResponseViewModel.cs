using System.ComponentModel.DataAnnotations;

namespace Diskussion.Models.ViewModels
{
    public class ResponseViewModel
    {
        public string IdDiscussion { get; set; }

        [Required(ErrorMessage = "El campo Mensaje es obligatorio")]
        public string Message { get; set; }
    }
}
