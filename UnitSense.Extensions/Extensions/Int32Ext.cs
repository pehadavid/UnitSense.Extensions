namespace UnitSense.Extensions.Extensions
{
    public  static class Int32Ext
    {
        /// <summary>
        /// Transforme un entier en sa valeur textuelle ordinale (1er, 2eme etc)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToOrdinal(this int num)
        {
            if (num == 1)
                return "1er";

            return $"{num}eme";
        }
    }
}