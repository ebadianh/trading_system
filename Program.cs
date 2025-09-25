using System.Reflection.Metadata;
using TradingApp;

List<User> users = new List<User>();
users.Add(new User("Havash", "1"));

User? active_user = null;

List<Item> items = new List<Item>();
items.Add(new Item("Stol", "Jag vill byta min gröna stol som är i bra skick. Jag har haft den i köket i tre år och inga småbarn har varit med i bilden"));


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


bool running = true;
while (running)

{
    if (active_user == null)
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();

        Console.Write("Password: ");
        string _password = Console.ReadLine();

        foreach (User user in users)
        {

            if (user.TryLogin(username, _password))
            {
                active_user = user;
                break;
            }
        }
    }
}

