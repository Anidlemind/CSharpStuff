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

  private const string EXIT_STRING = "q";
  private const string VALUE_INPUT_ERROR_MESSAGE = "Input value: \"{0}\" - is not a number.\n" +
                                                   "Consider that the Decimal point is represented by ','.";
  private const string UNSUPPORTED_OPERATION_ERROR_MESSAGE = "Unsupported operation: \"{0}\".\n" +
                                                             "Supported operations include: [{1}]";

  private static string Sum(double a, double b)
  {
    return (a + b).ToString();
  }

  private static string Diff(double a, double b)
  {
    return (a - b).ToString();
  }

  private static string Mult(double a, double b)
  {
    return (a * b).ToString();
  }

  private static string Div(double a, double b)
  {
    if (b == 0)
    {
      if (a == 0)
      {
        return "NaN";
      }
      if (a > 0)
      {
        return "+inf";
      }
      if (a < 0)
      {
        return "-inf";
      }
    }
    return (a / b).ToString();
  }

  private static string Exp(double a, double b)
  {
    return Math.Pow(a, b).ToString();
  }

  private static string Mod(double a, double b)
  {
    return (a % b).ToString();
  }

  private readonly string[] SUPPORTED_OPERATIONS = {"+", "-", "*", "/", "^", "%"};
  private readonly Func<double, double, string>[] OPERATION_MAPPING = {Sum, Diff, Mult, Div, Exp, Mod};

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
    Console.WriteLine($"The result of this operation is: {OPERATION_MAPPING[operationIndex](firstValue, secondValue)}\n" +
                      $"Waiting for any input to continue working or to close. (Exit command is \"{EXIT_STRING}\")");
    string input = Console.ReadLine();
    if (ExitCheck(input))
    {
      exitFlag = true;
    }
  }

  private void PrintUsage()
  {
    Console.WriteLine("This is a basic console calculator app that loops once an operation has been completed.\n" +
                      $"You can exit at any time by inputing: \"{EXIT_STRING}\".\n" +
                      $"The list of supported commands is: [{String.Join(',', SUPPORTED_OPERATIONS)}]\n");
  }

  public void MainLoop()
  {
    PrintUsage();
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
