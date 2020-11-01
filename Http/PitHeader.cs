namespace Pit.Http
{
    public class PitHeader
    {
        public string Key { get; }
        public string Value { get; }

        public PitHeader(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}