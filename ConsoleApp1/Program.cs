using System;
using System.Collections.Generic;

class Program
{
    static List<string> todoList = new List<string>();

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Todo List Uygulaması");
            Console.WriteLine("1. İş Ekle");
            Console.WriteLine("2. İşleri Listele");
            Console.WriteLine("3. İş Sil");
            Console.WriteLine("4. Çıkış");
            Console.Write("Seçiminizi yapın: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTodo();
                    break;
                case "2":
                    ListTodos();
                    break;
                case "3":
                    RemoveTodo();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                    break;
            }
        }
    }

    static void AddTodo()
    {
        Console.Write("Yeni iş ekleyin: ");
        string todo = Console.ReadLine();
        todoList.Add(todo);
        Console.WriteLine("İş eklendi! Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void ListTodos()
    {
        Console.WriteLine("Todo Listesi:");
        for (int i = 0; i < todoList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {todoList[i]}");
        }
        Console.WriteLine("Devam etmek için bir tuşa basın...");
        Console.ReadKey();
    }

    static void RemoveTodo()
    {
        ListTodos();
        Console.Write("Silmek istediğiniz işin numarasını girin: ");
        int index = Convert.ToInt32(Console.ReadLine()) - 1;
        if (index >= 0 && index < todoList.Count)
        {
            todoList.RemoveAt(index);
            Console.WriteLine("İş silindi! Devam etmek için bir tuşa basın...");
        }
        else
        {
            Console.WriteLine("Geçersiz numara. Devam etmek için bir tuşa basın...");
        }
        Console.ReadKey();
    }
}
