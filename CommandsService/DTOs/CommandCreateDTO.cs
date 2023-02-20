using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class CommandCreateDTO
    {
        [Required]
        public string? HowTo { get; set; }
        
        [Required]
        public string? CommandLine { get; set; }
    }
}