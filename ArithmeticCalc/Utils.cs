using System.Text.RegularExpressions;
namespace ArithmeticCalc
{
    internal class Utils
    {
        internal static bool isArab(string s)
        {
            var result = false;
            if (string.IsNullOrEmpty(s)) return result;
            var r = new Regex("^[0-9]+$");
            var matches = r.Match(s);
            result = matches.Success;                
            return result;
        }

        internal static bool isRoman(string s)
        {
            var result = false;
            if (string.IsNullOrEmpty(s)) return result;

            var r = new Regex(@"([XIVMCD])+");
            var matches = r.Match(s.ToUpper());
            result = matches.Success;
            return result;
        }

        internal static int GetFirstLetter(string s)
        {
            var result = -1;
            var r = new Regex("[a-zA-Z0-9]\\S+$");
            var matches = r.Match(s);
            if (matches.Success)
                result = matches.Index;
            return result;
        }

        internal static bool CheckBrackets(string input)
        {
            var result = false;
            if (input.IndexOf("(") == -1 && input.IndexOf(")") == -1)
                return result;

            Regex r = new Regex(@"\(");
            var matches = r.Matches(input);

            r = new Regex(@"\)");
            var matches2 = r.Matches(input);
            if (matches.Count == matches2.Count) result = true;

            return result;
        }

        internal static string GetCheckBrackets(string input)
        {
            var result = "";
            if (input.IndexOf("(") == -1 && input.IndexOf(")") == -1)
                return result;

            Regex r = new Regex(@"\(");
            var matches = r.Matches(input);

            r = new Regex(@"\)");
            var matches2 = r.Matches(input);

            if (matches.Count == matches2.Count) result = "";
            if (matches.Count > matches2.Count) result = "(";
            if (matches.Count < matches2.Count) result = ")";

            return result;
        }

        internal static int GetInt(string s, char op)
        {
            var result = 0;            
            if (string.IsNullOrEmpty(s)) return result;            
            var r = 0;
            if (int.TryParse(s, out r))
            {
                result = (op == '-')? -1*r :r;
            }
            return result;
        }
    }
}
