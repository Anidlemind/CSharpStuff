namespace CarListApp
{
  public enum CarType
  {
    Tesla,
    Toyota,
    Bmw,
    Lada
  }

  public interface ICar
  {
    string GetDescription();
  }

  public interface IElectric { }
  public interface IMechanicalEngine { } 
  public interface IAutomaticGearbox { }
  public interface IMechanicalGearbox { } 

  public abstract class ACar : ICar
  {
    public string brandName;
    public int seats;
    public string onBoard;
    private string EngineString()
    {
      return this switch
      {
        IElectric => "Машина на Электронике",
        IMechanicalEngine => "Машина на Механике",
        _ => "Неизвестная машина"
      };
    }
    private string GearboxString()
    { 
      return this switch
      {
        IAutomaticGearbox => "автоматической коробкой передач",
        IMechanicalGearbox => "механической коробкой передач",
        _ => "неизвестным типом коробки передач"
      };
    }

    protected ACar(string brandName, int seats, string onBoardSystem = "Отсутствует")
    {
      this.brandName = brandName;
      this.seats = seats;
      onBoard = onBoardSystem;
    }

    public virtual string GetDescription()
    {
      string engineType = EngineString();
      string gearboxType = GearboxString();

      string systemInfo = string.IsNullOrEmpty(onBoard) || onBoard == "Отсутствует" 
        ? "" 
        : $", {onBoard}";

      return $"{brandName}: {engineType} с {gearboxType}, {seats} местами{systemInfo}";
    }
  }

  public abstract class ElectricCarBase : ACar, IElectric
  {
    protected ElectricCarBase(string brandName, int seats, string onBoardSystem) 
      : base(brandName, seats, onBoardSystem) { }
  }

  public abstract class CombustionCarBase : ACar, IMechanicalEngine
  {
    protected CombustionCarBase(string brandName, int seats, string onBoardSystem) 
      : base(brandName, seats, onBoardSystem) { }
  }

  public class TeslaModel3 : ElectricCarBase, IAutomaticGearbox
  {
    public TeslaModel3() : base("Tesla", 5, "Андроид на борту") { }
  }

  public class ToyotaCamry : CombustionCarBase, IAutomaticGearbox
  {
    public ToyotaCamry() : base("Toyota", 5, "Мультимедиа на борту") { }
  }

  public class BmwM3 : CombustionCarBase, IMechanicalGearbox
  {
    public BmwM3() : base("BMW", 4, "iDrive система на борту") { }
  }

  public class LadaVesta : CombustionCarBase, IMechanicalGearbox
  {
    public LadaVesta() : base("Lada", 5, "Магнитола на борту") { }
  }

  public static class CarFactory
  {
    public static ICar CreateCar(CarType brand)
    {
      return brand switch
      {
        CarType.Tesla => new TeslaModel3(),
        CarType.Toyota => new ToyotaCamry(),
        CarType.Bmw => new BmwM3(),
        CarType.Lada => new LadaVesta(),
        _ => throw new ArgumentException($"Марка '{brand}' не поддерживается фабрикой.")
      };
    }
  }

  class Program
  {
    static void Main(string[] args)
    {   
      while (true)
      {
        Console.Write("\nВведите марку автомобиля (Tesla, Toyota, Bmw, Lada) или 'done' для остановки: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
          continue;

        if (input.ToLower() == "done")
        {
          Console.WriteLine("Программа завершена.");
          break;
        }

        if (Enum.TryParse<CarType>(input, ignoreCase: true, out var selectedBrand))
        {
          try
          {
            ICar car = CarFactory.CreateCar(selectedBrand);
            Console.WriteLine(car.GetDescription());
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Ошибка создания авто: {ex.Message}");
          }
        }
        else
        {
          Console.WriteLine("Неизвестная марка. Попробуйте снова.");
        }
      }
    }
  }
}