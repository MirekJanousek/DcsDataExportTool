using NLua;

namespace DcsExportLib.Extensions
{
    internal static class LuaTableExtensions
    {
        public static bool KeysAreOfType<T>(this LuaTable table)
        {
            if (table.Keys == null || table.Keys.Count == 0)
            {
                return false;
            }

            return table.Keys.OfType<T>().Count() == table.Keys.Count;
        }

        public static bool ValuesAreOfType<T>(this LuaTable table)
        {
            if (table.Values == null || table.Values.Count == 0)
            {
                return false;
            }

            return table.Values.OfType<T>().Count() == table.Values.Count;
        }

        public static bool ValuesAreNumbers(this LuaTable table)
        {
            return table.Values.OfType<decimal>().Count() + table.Values.OfType<long>().Count() + table.Values.OfType<double>().Count() == table.Values.Count;
        }

        public static bool AreCorrectTypes<T1, T2>(this LuaTable table)
        {
            bool areCorrectKeys = table.KeysAreOfType<T1>();

            if(!areCorrectKeys) 
                return false;

            bool areCorrectValues = table.ValuesAreOfType<T2>();
            
            return areCorrectValues;
        }
    }
}
