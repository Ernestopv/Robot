namespace Robot.Infrastructure.Settings
{
    public class AppSettings
    {
        public GPIO GPIO { get; set; } = null!;

        public bool Camera { get; set; }

        public double Speed { get; set; }

        public bool IsUpsAvailable { get; set; }

        public bool IsCameraAvailable { get; set; }

        public bool IsDevelopment { get; set; }
    }
}
