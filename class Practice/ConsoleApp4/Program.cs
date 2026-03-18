using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp4.Car;
using ConsoleApp4.CRUD;
using ConsoleApp4.Model;


namespace ConsoleApp4
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var product = (Id: 101, Name: "Laptop", Price: 75000);

            Console.WriteLine(product.Id);
            Console.WriteLine(product.Name);
            Console.WriteLine(product.Price);

            (string name, int id) User()
            {
                return ("ramesh", 1);
            }
            //touple in conditional logic
            (string Status, string Message) ValidateOrder(int qty)
            {
                if (qty <= 0)
                    return ("Error", "Quantity must be greater than zero");

                return ("Success", "Order placed");
            }

            var result = ValidateOrder(5);
            Console.WriteLine($"{result.Status}: {result.Message}");

            //touple collections
            var students = new List<(int Id, string Name, int Marks)>()
{
    (1, "Anil", 85),
    (2, "Sunil", 90),
    (3, "Ravi", 78)
};

            foreach (var s in students)
            {
                Console.WriteLine($"{s.Id} - {s.Name} - {s.Marks}");
            }
        }





        //string input = "hello";
        //Console.WriteLine(input.Length);

        //string input1 = "welcome";
        //Console.WriteLine(input1.ToUpper());

        //string input2 = "DOTNET";
        //Console.WriteLine(input2.ToLower());

        ////string input3 = "Hello";
        ////string input4 = "C#";
        ////Console.WriteLine($"{input3} {input4}");

        //string input3 = "Hello";
        //string input4 = "C#";
        //Console.WriteLine(input3+" "+input4);

        //string input5 = "";
        //Console.WriteLine(string.IsNullOrEmpty(input5));

        //string input6 = "India";
        //Console.WriteLine(input6[0]);

        //string input7 = "India";
        //Console.WriteLine(input7[input7.Length-1]);

        //string i = "abc";
        //string j = "ABC";
        //Console.WriteLine(i.Equals(j));

        //string input8 = "Welcome to C#";
        //string word = "C#";
        //string[] arr = input8.Split(' ');
        //foreach(string s in arr)
        //{
        //    Console.WriteLine(input8.Contains(s));
        //}

        //string input9 = " Hello World ";
        //Console.WriteLine(input9.Trim());

        //string input10 = "Csharp";
        //Char[] arr1 = input10.ToCharArray();
        //Array.Reverse(arr1);
        //string reversed = new string(arr1);
        //Console.WriteLine(reversed);

        //string e = "Education";
        //int count = 0;

        //foreach(char ch in e)
        //{
        //    if(ch == 'a'|| ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u' ||
        //       ch == 'A' || ch == 'E' || ch == 'I' || ch == 'O' || ch == 'U')
        //    {
        //        count++;
        //    }
        //}
        //Console.WriteLine($"Number of vowels in '{e}' is: {count}");

        //string q = "Hello";
        //int count1 = 0;

        //foreach(char ch in q)
        //{
        //    if(!(ch == 'a' || ch == 'e' || ch == 'i' || ch == 'o' || ch == 'u' ||
        //       ch == 'A' || ch == 'E' || ch == 'I' || ch == 'O' || ch == 'U'))
        //    {
        //        count1++;
        //    }
        //}
        //Console.WriteLine($"Number of consonants in '{q}' is: {count1}");

        //string p = "madam";
        //char[]arr3 = p.ToCharArray();
        //Array.Reverse(arr3);
        //string reversed1 = new string(arr3);
        //if(p.Equals(reversed1))
        //{
        //    Console.WriteLine($"{p} is a palindrome.");
        //}
        //else
        //{
        //    Console.WriteLine($"{p} is not a palindrome.");
        //}

        //string sentence = "I love C sharp";
        //string[]words = sentence.Split(' ');
        //int count4 = 0;
        //foreach(string word1 in words)
        //{
        //    count4++;
        //}
        //Console.WriteLine(count4);

        //string space = "Full Stack Developer";
        //char[] arr6 = space.ToCharArray();
        //string result = "";
        //foreach(char ch in arr6)
        //{
        //    if(ch == ' ')
        //    {
        //        result += "_";
        //    }
        //    else
        //    {
        //        result += ch;
        //    }
        //}
        //Console.WriteLine(result);

        //string firstOccur = "programming";
        //char target = 'g';
        //int index = -1;
        //int i1 = 0;

        //foreach(char s in firstOccur)
        //{
        //    if(s == target)
        //    {
        //        index = i1;
        //        break;
        //    }
        //    i1++;
        //}
        //Console.WriteLine(index);

        //string whitespace = "C Sharp Language";
        //string result1 = "";
        //foreach(char ch in whitespace)
        //{
        //    if(ch != ' ')
        //    {
        //        result1 += ch;
        //    }
        //}
        //Console.WriteLine(result1);

        //string startsub = "www.google.com";
        //string check = "www";
        //string[]arr12 = startsub.Split('.');

        //foreach(string s in arr12)
        //{
        //    if(s.Equals(check))
        //    {
        //        Console.WriteLine($"{startsub} starts with {check}");
        //        break;
        //    }
        //}

        //string endsub = "file.txt";
        //string check1 = "txt";
        //string[] arr21 = startsub.Split('.');

        //foreach (string s in arr21)
        //{
        //    if (!s.Equals(check))
        //    {
        //        Console.WriteLine($"{endsub} ends with {check1}");
        //        break;
        //    }
        //}

        //string eachchar = "banana";
        //char[] arr22 = eachchar.ToCharArray();
        //Dictionary<char,int> charcount = new Dictionary<char, int>();

        //foreach(char ch in arr22)
        //{
        //    if (charcount.ContainsKey(ch))
        //    {
        //        charcount[ch]++;
        //    }
        //    else
        //    {
        //        charcount[ch] = 1;
        //    }
        //}

        //foreach(var kvp in charcount)
        //{
        //    Console.WriteLine($"Character: {kvp.Key}, Count: {kvp.Value}");
        //}

        //string dup = "programming";
        //char[]arr23 = dup.ToCharArray();
        //string empt = "";

        //HashSet<char> seen = new HashSet<char>();

        //foreach(char ch in arr23)
        //{
        //    if (!seen.Contains(ch))
        //    {
        //        seen.Add(ch);
        //    }
        //}

        //foreach(char ch in seen)
        //{
        //    empt += ch;
        //}
        //Console.WriteLine(empt);

        //string nonrepeat = "swiss";
        //string result12 = "";
        //Dictionary<char, int> charcount1 = new Dictionary<char, int>();

        //foreach(char ch in nonrepeat)
        //{
        //    if (charcount1.ContainsKey(ch))
        //    {
        //        charcount1[ch]++;
        //    }
        //    else
        //    {
        //        charcount1[ch] = 1;
        //    }
        //}
        //foreach(char ch in nonrepeat)
        //{
        //    if (charcount1[ch] == 1)
        //    {
        //        result12 += ch;
        //        break;
        //    }
        //}
        //Console.WriteLine(result12);









        //CategoryCRUD crud = new CategoryCRUD();

        //Category category = new Category();

        //Console.WriteLine("enter Category name:");
        //category.Name = Console.ReadLine();

        //crud.AddCategory(category);

        //Console.WriteLine("Category added Successfully!");




        //            DeligateExample de = new DeligateExample();
        //            //Func, Action, Predicate
        //            List<int> results = new List<int>();
        //            Func<int, int> f1 = de.Square;
        //            f1 += de.Double;
        //            Console.WriteLine(f1(10));
        //            var Invoclist = f1.GetInvocationList().Cast<Func<int, int>>();
        //            foreach (var invlist in Invoclist)
        //            {
        //                int result = invlist.Invoke(10);
        //                results.Add(result);
        //            }

        //            foreach (int i in results)
        //            {
        //                Console.WriteLine(i);
        //            }
        //        }

        //        public class DeligateExample
        //        {
        //            public int Double(int x)
        //            {
        //                return x + x;
        //            }
        //            public int Square(int x)
        //            {
        //                return x * x;
        //            }

        //            public void Cube(int x)
        //            {
        //                Console.WriteLine(x * x * x);
        //            }
        //        }
        //    }
        //}

        //Delegates - built in public
        //    class DeligateExample
        //{
        //    public int Double(int x)
        //    {
        //        return x + x;
        //    }
        //    public int Square(int x)
        //    {
        //        return x * x;
        //    }

        //    public void Cube(int x)
        //    {
        //        Console.WriteLine(x * x * x);
        //    }

        //    public void Quad(int x)
        //    {
        //        Console.WriteLine(x * x * x * x);
        //    }

        //    public bool Eligible(int marks)
        //    {
        //        if (marks > 80)
        //            return true;
        //        else return false;
        //    }

        //}

        //Hashtable ht = new Hashtable();
        //ht.Add(1, "India");
        //ht.Add(2, "Japan");

        //foreach (DictionaryEntry item in ht)
        //{
        //    Console.WriteLine(item.Key + " " + item.Value);
        //}



        //string filePath = "example.txt";

        //try
        //{
        //    using (StreamReader reader = new StreamReader(filePath))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            Console.WriteLine(line);
        //        }
        //    }
        //}
        //catch (FileNotFoundException ex)
        //{
        //    Console.WriteLine($"Error: The file '{filePath}' was not found.");
        //}
        //catch (IOException ex)
        //{
        //    Console.WriteLine($"Error: {ex.Message}");
        //}



        //    string dirPath = "newDirectory";
        //    string filePath = dirPath + @"\example.text";

        //    try
        //    {
        //        // Create the directory if it doesn't exist
        //        if (!Directory.Exists(dirPath))
        //        {
        //            Directory.CreateDirectory(dirPath);
        //            Console.WriteLine("Directory created successfully.");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Directory already exists.");
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //    string[] lines = {
        //    "This is the first line.",
        //    "This is the second line.",
        //    "This is the third line."
        //};

        //    try
        //    {
        //        File.AppendAllLines(filePath, lines);
        //        Console.WriteLine("Lines written to file.");
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }


        //}





        //string filePath = "example.txt";
        //string additionalContent = "This is new content to append.";

        //try
        //{
        //    File.AppendAllText(filePath, additionalContent + Environment.NewLine);
        //    Console.WriteLine("Text appended to the file.");
        //}
        //catch (IOException ex)
        //{
        //    Console.WriteLine($"Error: {ex.Message}");
        //}

        //    string filePath = "example.txt";
        //    string[] lines = {
        //    "This is the first line.",
        //    "This is the second line.",
        //    "This is the third line."
        //};

        //    try
        //    {
        //        File.WriteAllLines(filePath, lines);
        //        Console.WriteLine("Lines written to file.");
        //    }
        //    catch (IOException ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }





        //string filename = "exampke.txt";
        //string[] lines =
        //{
        //    "This is first line.",
        //    "This is second line.",
        //    "This is third line.",

        //};
        //try
        //{
        //    File.WriteAllLines(filePath, lines);
        //    Console.WriteLine("its written all files");
        //}
        //catch
        //{
        //    Console.WriteLine(IOexception ex);
        //}




        //Encapsule e = new Encapsule("female",12);
        //string result = e.greeting();
        //Console.WriteLine(result);



        //Car car1 =new Maruti();
        //car1.Discount();
        //Car car = new BMW();
        //car.Discount();




        //string[] words = { "Dumb", "Idiot" };
        //string article = "test article ignores dumb";
        //bool check = article.ToUpper().Contains(words[0].ToUpper());

        //Console.WriteLine(check);

        //string [] w = { "my", "hi","me","xx" };

        //string s = "Heelo,my name is Nikhil . ! Kumar Sahu";

        //Console.WriteLine(s.Length); //str.length
        //Console.WriteLine(s.ToUpper());  //ToUpper
        //Console.WriteLine(s.ToLower());  //ToLower
        //Console.WriteLine(s.Substring(1,3)); //str.SubString()

        //foreach (string i in w) {
        //    Console.WriteLine(s.Contains(i));  //contains
        //}

        //string[] str = s.Split(' ', ',', '!');

        //foreach (string i in w)
        //{
        //    Console.WriteLine(i);
        //    Console.WriteLine(str.Contains(i)); //contains
        //}

        //string s1 = "Heelo,my name is Nikhil . ! Kumar Sahu";
        //Console.WriteLine(s1.IndexOf("name")); //IndexOf
        //Console.WriteLine(s1.IndexOf("n")); //IndexOf
        //Console.WriteLine(s1.IndexOf("zz")) ;//IndexOf

        //Console.WriteLine(s1.Replace('h','x')); //replace
        //Console.WriteLine(s1);

        //string s2 = "  !!!   He~elo,my n!ame is Nikhil . ! Kumar Sahu    ";
        //Console.WriteLine(s2);
        //Console.WriteLine(s2.Trim(' ')); //trim
        //Console.WriteLine(s2.Trim(' ','!'));

        //string s3 = "  !!!   He~elo,my n!ame is Nikhil . ! Kumar Sahu    ";

        //Console.WriteLine(s3);
        //string[] res = s3.Split('!', '~'); //split
        //Console.WriteLine(s3.Length);
        //Console.WriteLine(res.Length);
        //string result = "";
        //foreach (string m in res)
        //{
        //    result += m;
        //}
        //Console.WriteLine(result);
        //Console.WriteLine(result.Trim(' ')); //trim

        //Console.WriteLine(s3.StartsWith(" "));  //startWith
        //Console.WriteLine(s1.StartsWith("Heelo"));
        //Console.WriteLine(s1.StartsWith("xyz"));

        //Console.WriteLine(s1.EndsWith("Heelo"));  //EndsWith
        //Console.WriteLine(s1.EndsWith("Sahu"));

        //int marks = 100;
        //string subject = "maths";

        //string result123 = string.Format("I scored {0} marks in {1}", marks, subject);  //format
        //Console.WriteLine(result123);

        //int age = 25;
        //string name = "John";
        //string template = "My name is {0} and I am {1} years old.";
        //string result34 = string.Format(template, name, age);  //format

        //Console.WriteLine(result34);

        //string ss = "123";
        //ss.PadLeft(5, '0');
        //Console.WriteLine(ss.PadLeft(5, '0')); //PadLeft

        //string sss = "123";
        //string resultt = sss.PadRight(5, '*'); //Padright
        //Console.WriteLine(resultt);

        //string str1 = "Hello";
        //string str2 = "hello";
        //Console.WriteLine(str1.Equals(str2));  //equaks


        //string text = "Hello";
        //char ch = text[1];
        //Console.WriteLine(ch);  //Chars

        //string text1 = "";
        //Console.WriteLine(string.IsNullOrEmpty(text1)); //isnull or empty   

        //string text2 = "   ";
        //Console.WriteLine(string.IsNullOrWhiteSpace(text2)); //is null or whitespace

        //int result98 = string.Compare("apple", "banana");
        //Console.WriteLine(result98);

        //int result89 = string.Compare("apple", "apple");
        //Console.WriteLine(result89);  //Compare

        //int reee = "apple".CompareTo("Banana");
        //Console.WriteLine(reee);   // CompareTo

        //bool isEqual = "Hello".Equals("hello");
        //Console.WriteLine(isEqual); //Equals

        //int mp = string.CompareOrdinal("apple", "banana");
        //Console.WriteLine(mp); //apple comes before banana

        //Console.WriteLine(string.CompareOrdinal("Apple", "apple"));// 'A'=65 'a'=97  -32
        //Console.WriteLine(string.CompareOrdinal("apple", "Apple"));// 'A'=65 'a'=97  32


        //string tt = "Hello".Remove(1, 3);
        //Console.WriteLine(tt); // remove


        //string ans = "Hello".Insert(5, " World");
        //Console.WriteLine(ans); // Insert


        //string anss = string.Concat("Hello", " ", "World");
        //Console.WriteLine(anss); //concat



        //int value =0;
        //if ( value == 0)
        //{
        //    Console.WriteLine(DOW.Monday);
        //}

        //Area a = new Area(10);


        //Area a = new Area(10);
        //int result = a.CalculateArea();
        //Console.WriteLine(result);

        //Area a1 = new Area(10, 20);
        //int result1 = a1.CalculateArea();
        //Console.WriteLine(result1);

        //MySampleClass sc = new MySampleClass();

        //Patient p = new Patient();
        //p.Consulatant();
    }
    }


