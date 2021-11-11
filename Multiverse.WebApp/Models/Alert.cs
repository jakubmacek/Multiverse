namespace Multiverse.WebApp.Models
{
    public class Alert
    {
        public const string DefaultId = "default-alert";

        public string Id { get; set; } = DefaultId;
        public AlertType Type { get; set; }
        public string? Message { get; set; }
        public bool AutoClose { get; set; }
        public bool KeepAfterRouteChange { get; set; }
        public bool Fade { get; set; }
    }

    public enum AlertType
    {
        Success,
        Error,
        Info,
        Warning
    }
}