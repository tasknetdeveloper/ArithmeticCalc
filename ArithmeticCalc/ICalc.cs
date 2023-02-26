namespace ArithmeticCalc
{
    public interface ICalc
    {
        //int ToCalc(string s);
        (bool, int) ToCalc(string s);
    }
}
