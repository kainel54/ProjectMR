using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace ProjectMR.Core
{
    public static class Parser
    {
        public static string[] unit = new string[6]
        {
        "",
        "만 ",
        "억 ",
        "조 ",
        "경 ",
        "해 "
        };

        public static string ParseNumber(this BigInteger value, int unitCount = 2)
        {
            StringBuilder sb = new StringBuilder();
            Stack<BigInteger> number = new();

            if (value == 0) return "0";

            while (value > 0)
            {
                number.Push(value % 10000);
                value /= 10000;
            }

            for (int i = 0; i < unitCount; i++)
            {
                if (number.TryPeek(out BigInteger current))
                {
                    if (i == 0)
                    {
                        sb.Append(current);
                    }
                    else
                    {
                        sb.Append(current.ToString("D4"));
                    }

                    sb.Append(unit[number.Count - 1]);
                    number.Pop();
                }
            }

            return sb.ToString();
        }

        public static string ParseNumber(this ulong value, int unitCount = 2)
        {
            StringBuilder sb = new StringBuilder();
            Stack<ulong> number = new();

            while (value > 0)
            {
                number.Push(value % 10000);
                value /= 10000;
            }

            for (int i = 0; i < unitCount; i++)
            {
                if (number.TryPeek(out ulong current))
                {
                    sb.Append(current);
                    sb.Append(unit[number.Count - 1]);
                    number.Pop();
                }
            }

            return sb.ToString();
        }
    }
}

