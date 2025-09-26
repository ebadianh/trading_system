using System.Reflection.Metadata;
using TradingApp;

List<User> users = new List<User>();
users.Add(new User("Havash", "1"));

List<Item> items = new List<Item>();
items.Add(new Item("Stol", "Jag vill byta min gröna stol som är i bra skick. Jag har haft den i köket i tre år och inga småbarn har varit med i bilden"));


User? active_user = null;




bool running = true;
while (running)
{

    if (active_user == null)
    {
        Console.WriteLine("--- Welcome to the trading market ---");
        Console.WriteLine();
        Console.WriteLine("Please choose where you want to go");
        Console.WriteLine("1. I'm new and would like to have an account");
        Console.WriteLine("2. Login");

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
                else
                {
                    users.Add(new User(newUsername, newPassword));
                    Console.WriteLine("Added");
                }
                break;

            case "2":
                if (active_user == null)
                {
                    Console.WriteLine("Username: ");
                    string username = Console.ReadLine();

                    Console.WriteLine("Password: ");
                    string password = Console.ReadLine();

                    foreach (User user in users)
                    {
                        if (user.TryLogin(username, password))
                        {
                            active_user = user;
                            break;
                        }

                    }
                }
                break;
        }
    }
    else
    {
        Console.WriteLine("--- Welcome dear user ---");
        Console.WriteLine();
        Console.WriteLine("1. Upload item");
        Console.WriteLine("2. Browse trough items");
        Console.WriteLine("3. Request a trade from another");
        Console.WriteLine("4. Browse trade requests");
        Console.WriteLine("5. Accept or deny a request");
        Console.WriteLine("6. Browse trough completed trades");

        string Userinput = Console.ReadLine();

        switch (Userinput)
        {
            case "1":
                Console.WriteLine("Insert the name of your item");
                string ItemName = Console.ReadLine();

                if (ItemName == null)
                {
                    Console.WriteLine("Unvalid insert. please try again");
                    break;
                }
                Console.WriteLine("Give a short description of your item");
                string ItemDesc = Console.ReadLine();

                if (ItemDesc == null)
                {
                    Console.WriteLine("Unvalid insert, please try again");
                    break;
                }
                break;

        }
    }
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


///// 
///     active_user = user;
//Console.WriteLine("--- Welcome dear " + user.Email + " ---");
// Console.WriteLine("");
// Console.WriteLine("1. Upload your item you like to trade");
// Console.WriteLine("2. Browse through items from other users");
// Console.WriteLine("3. Logout");
