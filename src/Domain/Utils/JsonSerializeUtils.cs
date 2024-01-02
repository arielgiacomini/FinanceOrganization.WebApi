using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Domain.Utils
{
    public static class JsonSerializeUtils
    {
        public static string Serialize<TValue>(TValue value, bool useIdentityJsonInFile = false)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = useIdentityJsonInFile
            };

            var jsonString = JsonSerializer.Serialize(value, options);

            return jsonString;
        }
    }
}