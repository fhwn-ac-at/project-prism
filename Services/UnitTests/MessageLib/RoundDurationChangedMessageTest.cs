namespace UnitTests.MessageLib
{
    using global::MessageLib.Lobby;
    using System.Text.Json;

    [TestFixture]
    internal class RoundDurationChangedMessageTest
    {
        [Test]
        public void RoundDurationChanged()
        {
            var message = new RoundDurationChangedMessage(new RoundDurationChangedMessageBody(60));
            var serializedMessage = JsonSerializer.Serialize(message);
        }
    }
}
