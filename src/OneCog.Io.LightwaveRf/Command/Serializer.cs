using System.Text;

namespace OneCog.Io.LightwaveRf.Command
{
    public interface ISerializer
    {
        byte[] Serialize(string command);
    }

    internal class Serializer : ISerializer
    {
        public byte[] Serialize(string command)
        {
            return Encoding.UTF8.GetBytes(command);
        }
    }
}
