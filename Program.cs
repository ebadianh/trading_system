using System.Reflection.Metadata;
using TradingApp;

List<IUser> users = new List<IUser>();
users.Add(new IUser("Havash"));

List<Item> items = new List<Item>();
items.Add(new Item("Stol", "Jag vill byta min gröna stol som är i bra skick. Jag har haft den i köket i tre år och inga småbarn har varit med i bilden"));


// User
foreach (IUser user in users)
{
    Console.WriteLine(user.Name);
}

// Item + Description
foreach (Item item in items)
{
    Console.WriteLine(item.Info());
}

bool running = true;
while (running)
{






}


