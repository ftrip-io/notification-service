namespace ftrip.io.notification_service.NotificationTypes
{
    public static class NotificationTypeCodes
    {
        public const string AnswerOnReservation = "AnswerOnReservation";
        public const string NewReservation = "NewReservation";
        public const string CanceledReservation = "CanceledReservation";
        public const string GradedHost = "GradedHost";
        public const string GradedAccommodation = "GradedAccommodation";

        public static readonly string[] All = new[]
        {
            AnswerOnReservation,
            NewReservation,
            CanceledReservation,
            GradedHost,
            GradedAccommodation
        };
    }
}