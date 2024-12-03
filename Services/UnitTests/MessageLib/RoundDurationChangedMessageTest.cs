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
            RoundDurationChangedMessage message = new RoundDurationChangedMessage(new RoundDurationChangedMessageBody(60));
            string serializedMessage = JsonSerializer.Serialize(message);
        }
    }
}
