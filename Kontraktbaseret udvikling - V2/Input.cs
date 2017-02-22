using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontraktbaseret_udvikling___V2
{
    public class Input
    {
        public static int GetNumber(Action failCallBack = null)
        {
            int n;
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out n))
                    break;

                failCallBack?.Invoke();
            }
            return n;
        }

        public static int GetNumberBetween(int from, int to, Action failCallBack = null)
        {
            int n;
            while (true)
            {
                n = Input.GetNumber(failCallBack);
                if (n >= from && n <= to)
                    break;

                failCallBack?.Invoke();
            }
                
            return n;
        }

        public static string GetStringNotEmpty(Action failCallBack = null)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(input))
                    break;

                failCallBack?.Invoke();
            }
            return input;
        }

        public static string GetStringMaxLength(int maxLength, Action failCallBack = null)
        {
            string input;
            while (true)
            {
                input = Input.GetStringNotEmpty(failCallBack);
                if (input.Length <= maxLength)
                    break;

                failCallBack?.Invoke();
            }
            return input;
        }

        public static string GetStringEqualTo(Action failCallBack, params string[] param)
        {
            string input;
            while (true)
            {
                input = Input.GetStringNotEmpty(failCallBack);

                if (param.Any(obj => input.ToLower() == obj.ToLower()))
                    break;

                failCallBack?.Invoke();
            }
            return input;
        }

        public static void PressAnything()
        {
            Console.ReadLine();
            Console.Clear();
        }
    }
}
