using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterBot.Infrastructure
{
    public static class StringExtensions
    {
        public static bool BeginsWith(this string str, string other)
        {
            if (str.Length == 0)
                return false;

            if (other.Length > str.Length)
                return false;

            for (int i = 0; i < other.Length; i++)
            {
                if (str[i] != other[i])
                    return false;
            }

            return true;
        }
    }
}
