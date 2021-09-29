namespace Moerenuma.Test.Channels
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moerenuma.Channels.ChannelFacade;

  [TestClass]
  public sealed class UnboundedChannelFacadeTest : ChannelTestBase
  {
    protected override IChannel<T> Create<T>() => new UnboundedChannelFacade<T>();
  }
}
