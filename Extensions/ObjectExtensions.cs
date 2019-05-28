using Newtonsoft.Json;

namespace UnitSense.Extensions.Extensions
{
    public static class ObjectExtensions
    {
        public static TOut DownCast<TIn, TOut>(this TIn source)
        {
            var srcData = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<TOut>(srcData);
        }
    }
}
