namespace Kraft.Server.Services
{
    public class AppSettings : IAppSettings
    {
        public string RedisHost {get;set;}
    }

    public interface IAppSettings
    {
        string RedisHost { get; }
    }
}