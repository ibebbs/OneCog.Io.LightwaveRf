using System.Text;

namespace OneCog.Io.LightwaveRf.Response
{
    public interface ISerializer
    {
        string Deserialize(byte[] bytes);
    }

    internal class Serializer : ISerializer
    {
        public string Deserialize(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
