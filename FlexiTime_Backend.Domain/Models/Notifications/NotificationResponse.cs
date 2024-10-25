namespace FlexiTime_Backend.Domain.Models.Notifications
{
    public class NotificationResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public NotificationDto? Notification { get; set; }
    }

    // DTO (Data Transfer Object) pour renvoyer la notification avec les informations pertinentes
    public class NotificationDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
