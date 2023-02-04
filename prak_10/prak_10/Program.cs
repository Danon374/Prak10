using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace prak_10
{
    class Program
    {
        static List<User> users;
        const string path = "save.json";

        static void Main(string[] args)
        {
            users = Load();
            string currentUser = Auth();
            int userSelection = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("F1 - Добавить пользователя" + '\n' + "F2 - Удалить" + '\n' + "F3 - Обновить" + '\n' + "F4 - Поиск" + '\n' + "Escape - Сохранить");
                Console.WriteLine("Добро пожаловать, " + currentUser);
                Console.WriteLine("Все пользователи:");
                foreach (var user in users)
                {
                    Console.WriteLine("  " + user.Login);
                }
                Console.SetCursorPosition(0, 7 + userSelection);
                Console.WriteLine("->");
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (userSelection < users.Count - 1)
                            userSelection++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (userSelection > 0)
                            userSelection--;
                        break;
                    case ConsoleKey.Enter:
                        User selectedUser = users[userSelection];
                        Console.WriteLine("Логин: " + selectedUser.Login + ", пароль: " + selectedUser.Password);
                        Console.ReadKey();
                        break;
                    case ConsoleKey.F1:
                        Add();
                        break;
                    case ConsoleKey.F2:
                        users.RemoveAt(userSelection);
                        userSelection = 0;
                        break;
                    case ConsoleKey.F3:
                        Update(userSelection);
                        break;
                    case ConsoleKey.F4:
                        Search();
                        break;
                    case ConsoleKey.Escape:
                        Save();
                        return;
                }
            }
        }

        static List<User> Load()
        {
            List<User> users;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                users = JsonSerializer.Deserialize<List<User>>(json);
            }
            else
            {
                users = new List<User>
                {
                    new User("admin", "admin"),
                    new User("Danil", "1234"),
                    new User("Bob", "1111")
                };
            }
            return users;
        }

        static string Auth()
        {
            while (true)
            {
                Console.WriteLine("Введите логин");
                string currentUser = Console.ReadLine();
                Console.WriteLine("Введите пароль");
                string password = null;
                ConsoleKeyInfo info;
                while (true)
                {
                    info = Console.ReadKey(true);
                    if (info.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    password += info.KeyChar;
                    Console.Write('*');
                }
                Console.WriteLine();
                foreach (var user in users)
                {
                    if (currentUser == user.Login && password == user.Password)
                    {
                        return currentUser;
                    }
                }
                Console.WriteLine("Логин или пароль неверный");
            }
        }

        static void Add()
        {
            Console.Clear();
            Console.WriteLine("Введите логин");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль");
            string password = Console.ReadLine();
            users.Add(new User(login, password));
        }

        static void Update(int user)
        {
            Console.Clear();
            Console.WriteLine("Введите новый логин");
            string login = Console.ReadLine();
            Console.WriteLine("Введите новый пароль");
            string password = Console.ReadLine();
            users[user] = new User(login, password);
        }

        static void Search()
        {
            int fieldSelection = 0;
            bool search = true;
            while (search)
            {
                Console.Clear();
                Console.WriteLine("Выберите режим поиска");
                Console.WriteLine("  Логин");
                Console.WriteLine("  Пароль");
                Console.SetCursorPosition(0, fieldSelection + 1);
                Console.WriteLine("->");
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (fieldSelection > 0)
                            fieldSelection--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (fieldSelection < 1)
                            fieldSelection++;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.WriteLine("Введите значение для поиска");
                        string val = Console.ReadLine();
                        Console.WriteLine("Найдены следующие пользователи:");
                        switch (fieldSelection)
                        {
                            case 0:
                                foreach (var user in users)
                                {
                                    if (user.Login.Contains(val))
                                    {
                                        Console.WriteLine(user.Login);
                                    }
                                }
                                break;
                            case 1:
                                foreach (var user in users)
                                {
                                    if (user.Password.Contains(val))
                                    {
                                        Console.WriteLine(user.Login);
                                    }
                                }
                                break;
                        }
                        Console.ReadKey();
                        search = false;
                        break;
                }
            }
        }

        static void Save()
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(path, json);
        }
    }
}
