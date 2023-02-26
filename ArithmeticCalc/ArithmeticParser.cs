using NumeralSystemsConversion.Converter;
using System.Text.RegularExpressions;

namespace ArithmeticCalc
{
    public sealed class ArithmeticParser
    {
        internal string Result = "";
        private ICalc clc;
        public ArithmeticParser(ICalc calc)
        {
            clc = calc;
        }

        public (bool,int) EvaluateArab(string input)
        {
            var result = 0;
            var isValid = (Utils.CheckBrackets(input));
            if (!isValid) return (isValid, result);

            var s = GetSubItem(input);
            if (!string.IsNullOrEmpty(s))
            {
               (bool error,result) = clc.ToCalc(s);
                if (error) isValid = false;
            }
            return (isValid, result);
        }

        public (bool, string) Evaluate(string input)
        {
            var result = "";
            var isValid = (Utils.CheckBrackets(input));
            if (!isValid) return (isValid, result);

            var s = GetSubItem(input);
            if (!string.IsNullOrEmpty(s))
            {
                (bool error,int r) = clc.ToCalc(s);
                result = NumeralConverter.HinduArabicToRoman((r<0)?r*-1:r);
                if (r < 0)
                    result = $"-{result}";

                if(error) isValid = false;
            }
            return (isValid, result);
        }

        private string GetSubItem(string input)
        {
            if (!Utils.CheckBrackets(input)) return input;
            Regex r = new Regex( @"\(.*?\)");
            MatchCollection matches = r.Matches(input);
            var s = "";
            var error = false;
            foreach (Match match in matches)
            {
                (error,s) = GetSubSpecific(match);
                if (error) { return ""; }

                input = input.Remove(match.Index,match.Length);
                input = input.Insert(match.Index, s);
                break;                
            }

            if (Utils.CheckBrackets(input))
            {
               return  GetSubItem(input);
            }            
            return input;
        }

        private (bool, string) GetSubSpecific(Match match)
        {
            var result = "";
            var error = false;
            if (string.IsNullOrEmpty(match.Value)) return (error,result);

            var s = match.Value.Trim();
            var br = Utils.GetCheckBrackets(s);
            s = s.Replace("(","");
            s = s.Replace(")", "");
            var res = clc.ToCalc(s);
            result = res.Item2.ToString();
            error = res.Item1;

            if (br == "(") result = "(" + result;
            if (br == ")") result = result + ")";
            return (error, result);
        }
    }
}
