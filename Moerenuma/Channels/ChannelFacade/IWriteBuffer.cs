namespace Moerenuma.Channels.ChannelFacade
{
  public interface IWriteBuffer<T>
  {
    bool TryWrite(T item);
  }
}
