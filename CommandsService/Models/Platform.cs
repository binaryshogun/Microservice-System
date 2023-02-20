using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommandsService.Models
{
    public class Platform
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int PlatformId { get; set; }

        [Required]
        public string? Name { get; set; }

        public ICollection<Command> Commands { get; set; } = new List<Command>();
    }
}