using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AikaEmu.Shared.Utils
{
    public static class JsonUtil
    {
        public static void DeserializeFile<T>(string path, out T list)
        {
            if (!File.Exists(path))
            {
                throw new Exception($"File not found. {path}");
            }

            string content;
            try
            {
                using (var reader = File.OpenText(path))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw new Exception($"Could not open the file. {path}");
            }

            list = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        }

        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
    }
}