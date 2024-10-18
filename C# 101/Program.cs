using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C__101
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IProductService productService = new ProductService();
            Menu menu = new Menu(productService);
            menu.Run();
        }
    }
}

public class Product
{
    private string Name;
    private decimal Price;
    private int Quantity;
    public Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    public override string ToString()
    {
        return $"Name: {Name}, Price: {Price:C}, Quantity: {Quantity}";
    }
    public string GetName() => Name;
    public void SetName(string value) => Name = value;

    public decimal GetPrice() => Price;
    public void SetPrice(decimal value)
    {
        if (value > 0)
        {
            Price = value;
        }
    }

    public int GetQuantity() => Quantity;
    public void SetQuantity(int value)
    {
        if (value >= 0)
        {
            Quantity = value;
        }
    }
}

public interface IProductService
{
    void AddProduct(string name, decimal price, int quantity);
    void DisplayProducts();
    Product SearchProduct(string name);
    bool EditProduct(string name, decimal price, int quantity);
    bool DeleteProduct(string name);
}

public class ProductService : IProductService
{
    private readonly List<Product> products = new List<Product>();
    public void AddProduct(string name, decimal price, int quantity)
    {
        products.Add(new Product(name, price, quantity));
    }
    public void DisplayProducts()
    {
        if(products.Count == 0)
        {
            Console.WriteLine("No products available");
            return;
        }
        foreach (Product product in products)
        {
            Console.WriteLine(product);
        }
    }
    public Product SearchProduct(string name)
    {
        return products.Find(p => p.GetName().Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    public bool EditProduct(string name, decimal price, int quantity)
    {
        var product = SearchProduct(name);
        if(product != null)
        {
            product.SetPrice(price);
            product.SetQuantity(quantity);
            return true;
        }
        return false;
    }
    public bool DeleteProduct(string name)
    {
        var findProduct = SearchProduct(name);
        if(findProduct != null)
        {
            products.Remove(findProduct);
            return true;
        }
        return false;
    }
}

public class Menu
{
    private readonly IProductService productService;
    public Menu(IProductService productService)
    {
        this.productService = productService;
    }
    public void Run()
    {
        int choice;
        do
        {
            ShowMenu();
            choice = GetUserChoice();
            switch(choice)
            {
                case 1:
                    HandleAddProduct();
                    break;
                case 2:
                    productService.DisplayProducts();
                    break;
                case 3:
                    HandleSearchProduct();
                    break;
                case 4:
                    HandleEditProduct();
                    break;
                case 5:
                    HandleDeleteProduct();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (choice != 6);
    }
    private void ShowMenu()
    {
        Console.WriteLine("\n=== Product Management System ===");
        Console.WriteLine("1. Add Product");
        Console.WriteLine("2. Display All Products");
        Console.WriteLine("3. Search Product by Name");
        Console.WriteLine("4. Edit Product");
        Console.WriteLine("5. Delete Product");
        Console.WriteLine("6. Exit");
    }
    private int GetUserChoice()
    {
        Console.Write("Enter your choice: ");
        if (int.TryParse(Console.ReadLine(), out int choice)){
            return choice;
        }
        return -1;
    }

    private void HandleAddProduct()
    {
        Console.Write("Enter product name: ");
        string name = Console.ReadLine();
        decimal price = GetValidDecimal("Enter product price: ");
        int quantity = GetValidInt("Enter product quantity: ");
        productService.AddProduct(name, price, quantity);
    }
    private void HandleSearchProduct()
    {
        Console.Write("Enter product name to search: ");
        string name = Console.ReadLine();

        var product = productService.SearchProduct(name);
        if(product != null)
        {
            Console.WriteLine("Product found: ", product);
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
    private void HandleEditProduct()
    {
        Console.Write("Enter product name to edit: ");
        string name = Console.ReadLine();
        var product = productService.SearchProduct(name);
        if (product != null)
        {
            decimal newPrice = GetValidDecimal("Enter new price: ");
            int newQuantity = GetValidInt("Enter new quantity: ");
            productService.EditProduct(name, newPrice, newQuantity);
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }
    private void HandleDeleteProduct()
    {
        Console.Write("Enter product name to delete: ");
        string name = Console.ReadLine();

        if (productService.DeleteProduct(name))
        {
            Console.WriteLine("Product deleted successfully.");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
    }

    private decimal GetValidDecimal(string prompt)
    {
        decimal value;
        do
        {
            Console.Write(prompt);
        } while(!decimal.TryParse(Console.ReadLine(), out value));
        return value;
    }
    private int GetValidInt(string prompt)
    {
        int value;
        do
        {
            Console.Write(prompt);
        } while (!int.TryParse(Console.ReadLine(), out value));
        return value;
    }
}