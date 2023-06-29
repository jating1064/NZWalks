using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string userName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        public List<string> roles { get; set; }

    }
}
