namespace Kraft.Server
{
    public class AppSettings : IAppSettings
    {
        public string RedisHost {get;set;} = "localhost:6379";
    }

    public interface IAppSettings
    {
        string RedisHost { get; }
    }
}