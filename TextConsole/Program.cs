using ArithmeticCalc;

ArithmeticParser ap = new(new Calc());
var expression = "10+((1+2)-5)*(20-1)";
Console.WriteLine(expression);
var a = ap.EvaluateArab(expression);
Console.WriteLine($"input is valid :{a.Item1}  \r\nresult:{a.Item2}");



expression = "-X+I+((III+V)-IV)*(V-II)+(I*II)";
Console.WriteLine(expression);
var r = ap.Evaluate(expression);

Console.WriteLine($"input is valid :{r.Item1}  \r\nresult:{r.Item2}");
Console.ReadLine();