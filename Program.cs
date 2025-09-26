using System.Reflection.Metadata;
using TradingApp;

List<User> users = new List<User>();
users.Add(new User("Havash", "1"));

List<Item> items = new List<Item>();
items.Add(new Item("Stol", "Jag vill byta min gröna stol som är i bra skick. Jag har haft den i köket i tre år och inga småbarn har varit med i bilden"));


User? active_user = null;

// // User
// foreach (User user in users)
// {
//     Console.WriteLine(user.Email);
// }

// // Item + Description
// foreach (Item item in items)
// {
//     Console.WriteLine(item.Info());
// }

Console.Clear();
Console.WriteLine("--- Welcome to the trading market ---");
Console.WriteLine("");
Console.WriteLine("Please choose where you want to go");
Console.WriteLine("1. I'm new and would like to have an account");
Console.WriteLine("2. Login");
Console.WriteLine("3. Exit");

bool running = true;
while (running)
{
    bool Menurunning = true;
    while (Menurunning)

    {

        string menuinput = Console.ReadLine();

        switch (menuinput)
        {
            case "1":
                Console.WriteLine("Please choose your username");
                string newUsername = Console.ReadLine();

                Console.WriteLine("And enter your password");
                string newPassword = Console.ReadLine();

                bool exist = false;
                foreach (User user in users)
                {
                    if (user.NewUser() == newUsername)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist)
                {
                    Console.WriteLine("Username is already taken");
                }
                else if (exist = false)
                {
                    users.Add(new User(newUsername, newPassword));
                }
                else
                {
                    Console.WriteLine("Added");
                }
                break;

                // case "2":
                //     if (active_user == null)
                //     {
                //         Console.WriteLine("Username: ");
                //         string username = Console.ReadLine();

                //         Console.WriteLine("Password: ");
                //         string password = Console.ReadLine();

                //         foreach (User user in users)
                //         {
                //             if (user.TryLogin(username, password))
                //             {
                //                 active_user = user;
                //                 break;
                //             }
                //         }
                //     }
                //     else
                //     {
                //         Console.WriteLine("--- Welcome dear user ---");
                //     }
                //     break;

                // case "3":
                //     active_user = null;
                //     Menurunning = false;
                //     break;

                // default:
                //     Console.WriteLine("Invalid choice, try again please");
                //     break;


        }
    }

    // active_user = null;
    // Menurunning = false;
}
// if (active_user == null)
// {
//     Console.Write("Username: ");
//     string username = Console.ReadLine();

//     Console.Write("Password: ");
//     string _password = Console.ReadLine();

//     foreach (User user in users)
//     {

//         if (user.TryLogin(username, _password))
//         {
//             active_user = user;
//             break;
//         }
//     }
// }
// }

