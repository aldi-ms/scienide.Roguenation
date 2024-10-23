namespace scienide.Engine.Core.SoundSystem;

using scienide.Engine.Game.Actors;

public interface IMessageListener
{
    Actor Actor { get; set; }
    void OnMessageReceived(string message);
}
