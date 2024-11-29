namespace MessageLib
{
    using Newtonsoft.Json;

    // just here so I don't have to import Newtonsoft.Json anywhere and could change the implementation
    public static class Deserializer
    {
        public static T? DeserializeTo<T>(string? json) where T : class
        {
            if (json == null) return null;

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
