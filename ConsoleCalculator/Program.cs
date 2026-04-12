using System.Globalization;
// For converting Cyrilyc literals to English for special values in Div operation
using System.Text.RegularExpressions;
// For format validation

Calculator calculator = new Calculator();
calculator.MainLoop();


class Calculator
{
  private double firstValue;
  private double secondValue;
  private uint operationIndex;
  private bool exitFlag = false;
  // Considered exceptions - got a bit of a clumsy clump of code
  private bool errorFlag = false;
  private string FORMAT_STRING = "F4";


  private const string EXIT_STRING = "q";
  private const string VALUE_INPUT_ERROR_MESSAGE = "Input value: \"{0}\" - is not a number.\n" +
                                                   "Consider that the Decimal point is represented by ','.";
  private const string UNSUPPORTED_OPERATION_ERROR_MESSAGE = "Unsupported operation: \"{0}\".\n" +
                                                             "Supported operations include: [{1}]";
  private const string OUTPUT_MESSAGE = "The result of this operation is: {0}.\nWaiting for any input" +
                                        " to continue working or to close. (Exit command is \"{1}\")";
  private const string USAGE_STRING = "This is a basic console calculator app that loops once an operation" +
                                      "has been completed.\nYou can exit at any time by inputing: \"{0}\".\n" +
                                      "The list of supported commands is: [{1}]\n";
  private const string FORMAT_ASK_STRING = "Input double format for the output " +
                                           "(F5 for Fixed 5 digits, E5 for Exponential 5 digits)\n" +
                                           "Leave Empty for default format ({0})";
  private const string INVALID_FORMAT = "Ivalid Format, try again.";

  private static double Sum(double a, double b)
  {
    return a + b;
  }

  private static double Diff(double a, double b)
  {
    return a - b;
  }

  private static double Mult(double a, double b)
  {
    return a * b;
  }

  private static double Div(double a, double b)
  {
    return a / b;
  }

  private static double Exp(double a, double b)
  {
    return Math.Pow(a, b);
  }

  private static double Mod(double a, double b)
  {
    return a % b;
  }

  private readonly string[] SUPPORTED_OPERATIONS = {"+", "-", "*", "/", "^", "%"};
  private readonly Func<double, double, double>[] OPERATION_MAPPING = {Sum, Diff, Mult, Div, Exp, Mod};

  private bool ExitCheck(string inputValue)
  {
    return inputValue == EXIT_STRING;
  }

  private void HandleValueInput(ref double value)
  {
    string inputValue = Console.ReadLine();
    if (ExitCheck(inputValue)) {
      exitFlag = true;
      return;
    }
    try {
      value = Convert.ToDouble(inputValue);
    } catch {
      Console.WriteLine(String.Format(VALUE_INPUT_ERROR_MESSAGE, inputValue));
      errorFlag = true;
    }
  }

  private void HandleOperationInput()
  {
    string inputValue = Console.ReadLine();
    if (ExitCheck(inputValue)) {
      exitFlag = true;
      return;
    }
    for (uint i = 0; i < SUPPORTED_OPERATIONS.Length; ++i) {
      if (inputValue == SUPPORTED_OPERATIONS[i]) {
        operationIndex = i;
        return;
      }
    }
    Console.WriteLine(String.Format(UNSUPPORTED_OPERATION_ERROR_MESSAGE,
                                    inputValue, String.Join(',', SUPPORTED_OPERATIONS)));
    errorFlag = true;
  }

  private void EvaluateAndWait()
  {
    double result = OPERATION_MAPPING[operationIndex](firstValue, secondValue);
    Console.WriteLine(String.Format(OUTPUT_MESSAGE,
                                    result.ToString(FORMAT_STRING, CultureInfo.InvariantCulture),
                                    EXIT_STRING));
    // CultureInfo for Div special values
    string input = Console.ReadLine();
    if (ExitCheck(input))
    {
      exitFlag = true;
    }
  }

  private void PrintUsage()
  {
    Console.WriteLine(String.Format(USAGE_STRING, EXIT_STRING, String.Join(",", SUPPORTED_OPERATIONS)));
  }

  private void AskForFormat()
  {
    while (true) {
      Console.WriteLine(String.Format(FORMAT_ASK_STRING, FORMAT_STRING));
      string tmp = Console.ReadLine();
      if (tmp == string.Empty)
      {
        break;
      }
      if (!Regex.IsMatch(tmp, @"^[FE]\d+"))
      {
        Console.WriteLine(INVALID_FORMAT);
        continue;
      }
      FORMAT_STRING = tmp;
      break;
    }
  }

  public void MainLoop()
  {
    PrintUsage();
    AskForFormat();
    while (true)
    {
      Console.WriteLine("Input first value");
      HandleValueInput(ref firstValue);
      if (exitFlag || errorFlag)
      {
        break;
      }
      Console.WriteLine("Input second value");
      HandleValueInput(ref secondValue);
      if (exitFlag || errorFlag)
      {
        break;
      }
      Console.WriteLine("Input the operation");
      HandleOperationInput();
      if (exitFlag || errorFlag)
      {
        break;
      }
      EvaluateAndWait();
      if (exitFlag)
      {
        break;
      }
    }
  }
}
