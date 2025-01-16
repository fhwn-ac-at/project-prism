namespace UnitTests.MessageLib
{
    using global::MessageLib.Lobby;
    using global::MessageLib.SharedObjects;
    using System.Text.Json;

    [TestFixture]
    internal class RoundDurationChangedMessageTest
    {
        [Test]
        public void RoundDurationChanged()
        {
            DateTime dateTime = DateTime.Now;

            RoundDurationChangedMessage message = new RoundDurationChangedMessage(new RoundDurationChangedMessageBody(60), new MessageHeader(MessageType.roundDurationChanged , dateTime));
            string serializedMessage = JsonSerializer.Serialize(message);
        }
    }
}
