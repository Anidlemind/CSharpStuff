public class Program
{
  public static void Main()
  {
    List<int> ints = new() { 1, 2, 2, 3, 1, 4, 3 };
    List<string> strings = new() { "abc", "bac", "abc", "cab", "bac", "acb" };

    Console.WriteLine("Distinct int:");
    PrintList(CollectionUtils.Distinct(ints));

    Console.WriteLine("\nDistinct string:");
    PrintList(CollectionUtils.Distinct(strings));

    List<string> words = new() { "abc", "acb", "abcd", "acdb", "abdc", "ab" };
    Dictionary<int, List<string>> wordsByLength = CollectionUtils.GroupBy(words, word => word.Length);

    Console.WriteLine("\nWords by length:");
    PrintDictionaryOfLists(wordsByLength);

    Dictionary<string, int> firstCounters = new()
    {
      ["a"] = 2,
      ["b"] = 1,
      ["c"] = 3
    };

    Dictionary<string, int> secondCounters = new()
    {
      ["a"] = 4,
      ["d"] = 5,
      ["c"] = 1
    };

    Dictionary<string, int> mergedCounters = CollectionUtils.Merge(
      firstCounters,
      secondCounters,
      (firstValue, secondValue) => firstValue + secondValue);

    Console.WriteLine("\nMerged counters:");
    PrintDictionary(mergedCounters);

    List<Product> products = new()
    {
      new Product(1, "Product1", 1m),
      new Product(2, "Product2", 2m),
      new Product(3, "Product3", 3m)
    };

    Product mostExpensiveProduct = CollectionUtils.MaxBy(products, product => product.Price);

    Console.WriteLine($"\nMost expensive product: {mostExpensiveProduct}");
  }

  private static void PrintList<T>(List<T> items)
  {
    foreach (T item in items)
    {
      Console.Write($"{item} ");
    }

    Console.WriteLine();
  }

  private static void PrintDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
  {
    foreach (KeyValuePair<TKey, TValue> pair in dictionary)
    {
      Console.WriteLine($"{pair.Key}: {pair.Value}");
    }
  }

  private static void PrintDictionaryOfLists<TKey, TValue>(Dictionary<TKey, List<TValue>> dictionary) where TKey : notnull
  {
    foreach (KeyValuePair<TKey, List<TValue>> pair in dictionary)
    {
      Console.Write($"{pair.Key}: ");
      PrintList(pair.Value);
    }
  }
}

public static class CollectionUtils
{
  public static List<T> Distinct<T>(List<T> source)
  {
    List<T> result = new();
    HashSet<T> seen = new();

    foreach (T item in source)
    {
      if (seen.Add(item))
      {
        result.Add(item);
      }
    }

    return result;
  }

  public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
    List<TValue> source,
    Func<TValue, TKey> keySelector) where TKey : notnull
  {
    Dictionary<TKey, List<TValue>> result = new();

    foreach (TValue item in source)
    {
      TKey key = keySelector(item);

      if (!result.ContainsKey(key))
      {
        result[key] = new List<TValue>();
      }

      result[key].Add(item);
    }

    return result;
  }

  public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
    Dictionary<TKey, TValue> first,
    Dictionary<TKey, TValue> second,
    Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull
  {
    Dictionary<TKey, TValue> result = new(first);

    foreach (KeyValuePair<TKey, TValue> pair in second)
    {
      if (result.TryGetValue(pair.Key, out TValue? existingValue))
      {
        result[pair.Key] = conflictResolver(existingValue, pair.Value);
      }
      else
      {
        result[pair.Key] = pair.Value;
      }
    }

    return result;
  }

  public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
    where TKey : IComparable<TKey>
  {
    if (source.Count == 0)
    {
      throw new InvalidOperationException("Collection is empty.");
    }

    T maxItem = source[0];
    TKey maxKey = selector(maxItem);

    for (int i = 1; i < source.Count; i++)
    {
      T currentItem = source[i];
      TKey currentKey = selector(currentItem);

      if (currentKey.CompareTo(maxKey) > 0)
      {
        maxItem = currentItem;
        maxKey = currentKey;
      }
    }

    return maxItem;
  }
}

public class Product
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
