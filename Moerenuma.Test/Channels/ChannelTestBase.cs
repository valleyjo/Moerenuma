namespace Moerenuma.Test.Channels
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using FluentAssertions;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Moerenuma.Channels.ChannelFacade;

  [TestClass]
  public abstract class ChannelTestBase
  {
    protected ChannelTestBase()
    {
    }

    [TestMethod]
    public void ReadThenWrite()
    {
      IChannel<string> channel = this.Create<string>();
      ValueTask<string> pending = ReadPending(channel);
      Write(channel, "unblock");
      pending.Result.Should().Be("unblock");
    }

    [TestMethod]
    public void WriteThenRead()
    {
      IChannel<string> channel = this.Create<string>();
      Write(channel, "no wait");
      Read(channel, "no wait");
    }

    [TestMethod]
    public void DisposeThenRead()
    {
      IChannel<string> channel = this.Create<string>();
      channel.Dispose();
      ReadFailed<string, ObjectDisposedException>(channel);
    }

    [TestMethod]
    public void DisposeThenWrite()
    {
      IChannel<string> channel = this.Create<string>();
      channel.Dispose();
      WriteFailed<string, ObjectDisposedException>(channel, "fail");
    }

    [TestMethod]
    public void DisposeTwice()
    {
      IChannel<string> channel = this.Create<string>();
      channel.Dispose();
      Action act = () => channel.Dispose();
      act.Should().NotThrow();
    }

    [TestMethod]
    public void ReadThenCancel()
    {
      IChannel<string> channel = this.Create<string>();
      using var cts = new CancellationTokenSource();
      ValueTask<string> pending = ReadPending<string>(channel, cts.Token);
      cts.Cancel();
      pending.IsCanceled.Should().BeTrue();
    }

    [TestMethod]
    public void CancelThenRead()
    {
      IChannel<string> channel = this.Create<string>();
      using var cts = new CancellationTokenSource();
      cts.Cancel();
      ReadFailed<string, OperationCanceledException>(channel, cts.Token);
    }

    protected static void Read<T>(IReadBuffer<T> buffer, T expected)
    {
      ValueTask<T> result = buffer.ReadAsync(CancellationToken.None);
      result.IsCompletedSuccessfully.Should().BeTrue();
      result.Result.Should().Be(expected);
    }

    protected static void ReadFailed<T, TException>(IReadBuffer<T> buffer, CancellationToken token = default)
        where TException : Exception
    {
      ValueTask<T> result = buffer.ReadAsync(token);
      result.IsCompleted.Should().BeTrue();
      result.IsCompletedSuccessfully.Should().BeFalse();
      Action act = () => _ = result.Result;
      act.Should().Throw<TException>();
    }

    protected static ValueTask<T> ReadPending<T>(IReadBuffer<T> buffer, CancellationToken token = default)
    {
      ValueTask<T> result = buffer.ReadAsync(token);
      result.IsCompleted.Should().BeFalse();
      return result;
    }

    protected static void Write<T>(IWriteBuffer<T> buffer, T item)
    {
      bool result = buffer.TryWrite(item);
      result.Should().BeTrue();
    }

    protected static void WriteFailed<T, TException>(IWriteBuffer<T> buffer, T item)
        where TException : Exception
    {
      Action act = () => buffer.TryWrite(item);
      act.Should().Throw<TException>();
    }

    protected abstract IChannel<T> Create<T>();
  }
}
