namespace CommandsService.DTOs
{
    public class CommandReadDTO
    {
        public int Id { get; set; }

        public string? HowTo { get; set; }

        public string? CommandLine { get; set; }

        public int PlatformId { get; set; }
    }
}