namespace UnitSense.Extensions.Extensions
{
    public static class DynamicExt
    {
        public static T GetValueType<T>(this object dyn)
        {
            return (T)dyn;
        }
    }
}
