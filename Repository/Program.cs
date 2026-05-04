public class Program
{
  public static void Main()
  {
    Repository<Product> productRepository = new();
    productRepository.Add(new Product(1, "Product1", 1m));
    productRepository.Add(new Product(2, "Product2", 2m));
    productRepository.Add(new Product(3, "Product3", 3m));

    Console.WriteLine("GetAll:");
    PrintList(productRepository.GetAll());

    Console.WriteLine();
    Console.WriteLine($"GetById(2): {productRepository.GetById(2)}");
    Console.WriteLine($"Count: {productRepository.Count}\n");

    Console.WriteLine("Predicate (Price > 1):");
    PrintList(productRepository.Find(product => product.Price > 1m));

    Repository<User> userRepository = new();
    userRepository.Add(new User(1, "User1", "user1@example.com"));
    userRepository.Add(new User(2, "User2", "user2@example.com"));

    Console.WriteLine("\nAll users:");
    PrintList(userRepository.GetAll());

    Console.WriteLine();
    Console.WriteLine($"Remove(1): {productRepository.Remove(1)}");
    Console.WriteLine($"Remove(10): {productRepository.Remove(10)}");
    Console.WriteLine($"Count: {productRepository.Count}\n");

    try
    {
      productRepository.Add(new Product(2, "Product2.v2", 2m));
    }
    catch (InvalidOperationException exception)
    {
      Console.WriteLine($"\nDuplicate add error: {exception.Message}");
    }
  }

  private static void PrintList<T>(IReadOnlyList<T> items)
  {
    foreach (T item in items)
    {
      Console.WriteLine(item);
    }
  }
}

public interface IEntity
{
  int Id { get; }
}

public class Repository<T> where T : IEntity
{
  private readonly Dictionary<int, T> items = new();

  public int Count
  {
    get { return items.Count; }
  }

  public void Add(T item)
  {
    if (items.ContainsKey(item.Id))
    {
      throw new InvalidOperationException($"Id {item.Id} is taken.");
    }

    items.Add(item.Id, item);
  }

  public bool Remove(int id)
  {
    return items.Remove(id);
  }

  public T? GetById(int id)
  {
    return items.GetValueOrDefault(id);
  }

  public IReadOnlyList<T> GetAll()
  {
    return items.Values.ToList();
  }

  public IReadOnlyList<T> Find(Predicate<T> predicate)
  {
    return items.Values.Where(item => predicate(item)).ToList();
  }
}

public class Product : IEntity
{
  public int Id { get; }
  public string Name { get; }
  public decimal Price { get; }

  public Product(int id, string name, decimal price)
  {
    Id = id;
    Name = name;
    Price = price;
  }

  public override string ToString()
  {
    return $"{Id}: {Name}, price: {Price}";
  }
}

public class User : IEntity
{
  public int Id { get; }
  public string Name { get; }
  public string Email { get; }

  public User(int id, string name, string email)
  {
    Id = id;
    Name = name;
    Email = email;
  }

  public override string ToString()
  {
    return $"{Id}: {Name}, email: {Email}";
  }
}
