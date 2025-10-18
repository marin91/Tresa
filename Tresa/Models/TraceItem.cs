namespace Tresa.Models
{
    public class TraceItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FilePath { get; set; } = string.Empty; // PNG or SVG path
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
