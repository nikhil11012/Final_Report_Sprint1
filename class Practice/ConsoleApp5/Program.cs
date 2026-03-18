using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp5.CRUD;
using ConsoleApp5.Models;

namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. Update Product");
            Console.WriteLine("3. Delete Product");
            Console.WriteLine("4.Fetch");
            Console.WriteLine("5. Exit");
            Console.WriteLine("--------------------------------------------------------------------------------");

            bool check = true;
            while (check)
            {
                Console.WriteLine("Enter your choice:");
                int choice = int.Parse(Console.ReadLine());
                Product product = new Product();
                ProductCRUD productCRUD = new ProductCRUD();    
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("enter Product name:");
                        product.Name = Console.ReadLine();
                        Console.WriteLine("enter CategoryId:");
                        product.CategoryId = int.Parse(Console.ReadLine());
                        Console.WriteLine(productCRUD.AddProduct(product));
                        break;

                    case 2:
                        Console.WriteLine("Enter Product Id to update:");
                        product.ProductId = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter Product name:");
                        product.Name = Console.ReadLine();

                        Console.WriteLine("Enter CategoryId:");
                        product.CategoryId = int.Parse(Console.ReadLine());

                        Product updatedProduct = productCRUD.UpdateProduct(product);
                        Console.WriteLine($"Updated -> ID: {updatedProduct.ProductId} | Name: {updatedProduct.Name} | CategoryId: {updatedProduct.CategoryId}");
                        break;

                    case 3:
                        Console.WriteLine("Enter Product Id to delete:");
                        product.ProductId = int.Parse(Console.ReadLine());

                        Console.WriteLine(productCRUD.DeleteProduct(product));
                        break;
                    case 4:
                        List<Product> plist = productCRUD.GetProducts();

                        foreach (Product pr in plist)
                        {
                            Console.WriteLine($"ID: {pr.ProductId} | Name: {pr.Name} | CategoryId: {pr.CategoryId}");
                        }
                        break;
                    case 5:
                        check = false;
                        break;
                }
            }



            //CategoryCRUD crud = new CategoryCRUD();

            //Category category = new Category();

            //Console.WriteLine("enter Category name:");
            //category.Name = Console.ReadLine();

            //crud.AddCategory(category);

            //Console.WriteLine("Category added Successfully!");

            //category.Id = 3;
            //category.Name = "Mobile";
            //String result = crud.UpgradeCategory(category);
            //Console.WriteLine(result);

            //category.Id = 4;
            //String result1 = crud.DeleteCategory(category);
            //Console.WriteLine(result1);

            //List<Category> clist = crud.CategoriesList();
            //foreach (Category ct in clist) { 
            //    //Console.WriteLine(ct.Id + " " + ct.Name);
            //    Console.WriteLine($"ID: {ct.Id} | Name: {ct.Name}");


            //}

            //ProductCRUD pc = new ProductCRUD();
            //Product product = new Product()
            //{
            //    Name="Dosa",
            //    CategoryId= 1

            //};
            //Console.WriteLine(pc.AddProduct(product));
            //Product product1 = new Product()
            //{
            //    ProductId = 1,
            //    Name = "Idly",
            //    CategoryId = 1

            //};
            //Product p = pc.UpdateProduct(product1);
            //Console.WriteLine($"ID: {p.ProductId} | Name: {p.Name} | CategoryId: {p.CategoryId}");

            //Console.WriteLine(pc.DeleteProduct(1));

            //List<Product> plist = pc.GetProducts();
            //foreach (Product pr in plist)
            //{
            //    Console.WriteLine($"ID: {pr.ProductId} | Name: {pr.Name} | CategoryId: {pr.CategoryId}");
            //}

        }
    }
}
