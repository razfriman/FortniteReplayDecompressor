using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unreal.Core.Extensions
{
    public static class Utilities
    {
        public static string RemoveAllPathPrefixes(string path)
        {
            path = RemovePathPrefix(path, "Default__");

            for (int i = path.Length - 1; i >= 0; i--)
            {
                switch (path[i])
                {
                    case '.':
                        return path.Substring(i + 1);
                    case '/':
                        return path;
                }
            }

            return path;
        }

        //Removes all numbers and underscores from suffix
        private static string CleanPathSuffix(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                bool isDigit = (path[i] ^ '0') <= 9;
                bool isUnderscore = path[i] == '_';

                if (!isDigit && !isUnderscore)
                {
                    return path.Substring(0, i + 1);
                }
            }

            return path;
        }

        private static string RemovePathPrefix(string path, string toRemove)
        {
            if (toRemove.Length > path.Length)
            {
                return path;
            }

            for (int i = 0; i < toRemove.Length; i++)
            {
                if (path[i] != toRemove[i])
                {
                    return path;
                }
            }

            return path.Substring(toRemove.Length);
        }
    }
}
