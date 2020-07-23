using _Project.Scripts.Networking.ByteArray;

namespace _Project.Scripts.Networking.Datagram
{
    public static class DatagramTemplates
    {
        #region server
        public static byte[] WriteWelcomeMessage(int clientId) =>
            new ByteArrayBuilder()
                .Write((int) ServerDatagram.WelcomeDatagram)
                .Write(clientId)
                .InsertSize()
                .ToByteArray();

        public static string ReadWelcomeReceivedMessage(ByteArrayReader datagram) => datagram.ReadString();

        #endregion

        #region client

        public static byte[] WriteDummyMessage() =>
            new ByteArrayBuilder()
                .InsertSize()
                .ToByteArray();
        
        public static byte[] WriteWelcomeReceivedMessage(string username) =>
            new ByteArrayBuilder()
                .Write((int) ClientDatagram.WelcomeReceived)
                .Write(username)
                .InsertSize()
                .ToByteArray();
        
        public static int ReadWelcomeMessage(ByteArrayReader datagram) => datagram.ReadInt();

        #endregion
    }
}