using Il2CppSystem.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTools.src {
    public static class Extensions {
        public static List<T> ToList<T>(this Il2CppSystem.Collections.Generic.IEnumerable<T> dirtyList) {
            // idk what this does, vs just gave me this
            List<T> list = [.. dirtyList.ToArray()];
            return list;
        }
    }
}
