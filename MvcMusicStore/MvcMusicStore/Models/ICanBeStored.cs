namespace MvcMusicStore.Models
{
    public interface ICanBeStored
    {
        string Id { get; set; }
        string Name { get; }
    }
}