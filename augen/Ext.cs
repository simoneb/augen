using System.Linq;

namespace augen
{
    public static class Ext
    {
         public static ILookup<string, object> Merge(this ILookup<string, object> instance, ILookup<string, object> other)
         {
             return (from entry in instance.Concat(other)
                     let key = entry.Key
                     from element in entry
                     select new {key, element}).ToLookup(a => a.key, a => a.element);
         }
    }
}