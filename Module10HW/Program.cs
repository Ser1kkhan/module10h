using System;
using System.IO;

public interface ICalculatable
{
    double Add(double a, double b);
    double Subtract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);
}

public interface IStorable
{
    void SaveState(string filePath);
    void LoadState(string filePath);
}

public class SimpleCalculator : ICalculatable
{
    public double Add(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;
    public double Divide(double a, double b)
    {
        if (b == 0) throw new DivideByZeroException("Division by zero is not allowed.");
        return a / b;
    }

    protected void DisplayResult(double result)
    {
        Console.WriteLine($"Result: {result}");
    }
}

public class AdvancedCalculator : SimpleCalculator, IStorable, ICalculatable
{
    private double lastResult;

    public double Power(double a, double b)
    {
        lastResult = Math.Pow(a, b);
        return lastResult;
    }

    public double SquareRoot(double a)
    {
        lastResult = Math.Sqrt(a);
        return lastResult;
    }

    public void SaveState(string filePath)
    {
        File.WriteAllText(filePath, lastResult.ToString());
    }

    public void LoadState(string filePath)
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            lastResult = double.Parse(content);
        }
        else
        {
            throw new FileNotFoundException("The file was not found.", filePath);
        }
    }

    public new void DisplayResult(double result)
    {
        base.DisplayResult(result);
    }
}

class Program
{
    static void Main()
    {
        SimpleCalculator simpleCalc = new SimpleCalculator();
        Console.WriteLine("Simple Calculator:");
        Console.WriteLine(simpleCalc.Add(10, 5));
        Console.WriteLine(simpleCalc.Subtract(10, 5));
        Console.WriteLine(simpleCalc.Multiply(10, 5));
        Console.WriteLine(simpleCalc.Divide(10, 5));

        AdvancedCalculator advancedCalc = new AdvancedCalculator();
        Console.WriteLine("\nAdvanced Calculator:");
        advancedCalc.DisplayResult(advancedCalc.Add(20, 10));
        advancedCalc.DisplayResult(advancedCalc.Subtract(20, 10));
        advancedCalc.DisplayResult(advancedCalc.Multiply(20, 10));
        advancedCalc.DisplayResult(advancedCalc.Divide(20, 10));
        advancedCalc.DisplayResult(advancedCalc.Power(2, 3));
        advancedCalc.DisplayResult(advancedCalc.SquareRoot(16));

        string filePath = "calculatorState.txt";
        advancedCalc.SaveState(filePath);

        AdvancedCalculator anotherCalc = new AdvancedCalculator();
        anotherCalc.LoadState(filePath);
        Console.WriteLine("\nLoaded state in another calculator instance:");
        anotherCalc.DisplayResult(anotherCalc.Power(2, 3)); 

        Console.ReadKey();
    }
}
