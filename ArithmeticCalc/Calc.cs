using NumeralSystemsConversion.Converter;
using System.Collections;

namespace ArithmeticCalc
{
    public class Calc : ICalc
    {
        private Stack st = new();
        private Stack stMulti = new();
        private Queue qu = new();

        private void MoveMultiStackToCommonStack()
        {
            var r = 0;
            while (stMulti.Count > 0)
            {
                var obj = stMulti.Pop();
                if (obj != null)
                {
                    if (r == 0) r = 1;
                    r *= (int)obj;
                }
            }
            st.Push(r);
        }

        private void InStack(ArithOperator op, string val, int i)
        {
            if (string.IsNullOrEmpty(val)) return;
            char aoperator = '+';
            if (i < 0)
                aoperator = '-';

            if (op == ArithOperator.tostack)
            {
                MoveMultiStackToCommonStack();
                
                st.Push(GetInt(val, aoperator));
            }
            else if (op == ArithOperator.fromstack)
            {
                stMulti.Push(GetInt(val, aoperator));                
            }
        }

        private string GetNormalizeExpress(string s)
        {
            var result = "";
            qu.Clear();
            if (string.IsNullOrEmpty(s)) return  result;
            s = s.Replace(" ","");
            s = s.Replace("+-", "-");
            if (s.First() != '+' && s.First() != '-')
                s = $"+{s}";
            var i = 0;
            foreach (var item in s)
            {
                if (item == '-')
                {
                    qu.Enqueue(-1);
                }
                else if (item == '+')
                    qu.Enqueue(1);
                else if (item == '*' && s[i+1]!='-')
                    qu.Enqueue(1);

                result += (item == '-') ? '+' : item;
                i++;
            }
            result = result.Replace("*+","*");
            
            return result;
        }

        public (bool, int) ToCalc(string s)
        {
            st.Clear();
            stMulti.Clear();

            s = GetNormalizeExpress(s);

            var result = 0;
            var error = false;

            if (string.IsNullOrEmpty(s)) return (true, result);
            if (qu == null) return (true,result);
            var first = "";
            var preresult = 0;
            var aoperator = ' ';
            var item0 = ' ';
            var op = ArithOperator.empty;
            int? x = 0;
            foreach (var item in s)
            {

                item0 = item;
                if (op != ArithOperator.empty)
                    first = "";

                if (item0 == '+' || item0 == '*')
                {
                    if (item0 == '+' && aoperator == ' ' && !string.IsNullOrEmpty(first))
                    {
                        x = (int?)qu.Dequeue();
                        InStack(ArithOperator.tostack, first, x??1);
                        first = "";
                    }
                    if (item0 == '+' && aoperator == '*' && !string.IsNullOrEmpty(first))
                    {
                        x = (int?)qu.Dequeue();
                        InStack(ArithOperator.fromstack, first, x ?? 1);
                        first = ""; 
                    }

                    if (item0 == '+' && aoperator == '+' && !string.IsNullOrEmpty(first))
                    {
                        x = (int?)qu.Dequeue();
                        InStack(ArithOperator.tostack, first, x ?? 1);
                        first = ""; 
                    }
                    
                    if (item0 == '*' && aoperator == '+' && !string.IsNullOrEmpty(first))
                    {
                        x = (int?)qu.Dequeue();
                        InStack(ArithOperator.fromstack, first, x ?? 1);
                        first = "";
                    }
                    if (item0 == '*' && aoperator == '*' && !string.IsNullOrEmpty(first))
                    {
                        x = (int?)qu.Dequeue();
                        InStack(ArithOperator.fromstack, first, x ?? 1);
                        first = "";
                    }
                    aoperator = item0;
                }
                else
                {
                    first += item;
                    op = ArithOperator.empty;
                }
            }

            if (qu != null && qu.Count>0)            
                x = (int?)qu.Dequeue();
            else
                x=aoperator;

            if (aoperator == '*' && !string.IsNullOrEmpty(first))
            {
                InStack(ArithOperator.fromstack, first, x ?? 1);
                MoveMultiStackToCommonStack();
            }
            else
                InStack(ArithOperator.tostack, first, x ?? 1);

            preresult = 0;
            while (st.Count > 0)
            {
                var obj = st.Pop();
                if (obj != null)
                    preresult += (int)obj;
            }
            result = preresult;
            return (error, result);
        }

        private int GetInt(string s,char op)
        {
            var result = 0;
            var i = 1;
            if (op == '-')
            {
                i = -1;
            }

            if (Utils.isRoman(s))
                result = NumeralConverter.RomanToHinduArabic(s);
            else if (Utils.isArab(s))
                int.TryParse(s, out result);

            return result * i;
        }
    }
}
