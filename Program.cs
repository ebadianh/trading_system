using System.Reflection.Metadata;
using Microsoft.VisualBasic;
using TradingApp;

User? active_user = null;

User testUser1 = new User("Havash", "1");
User testUser2 = new User("Sofie", "1");
User testUser3 = new User("Belle", "1");

List<User> users = new List<User>();
users.Add(testUser1);
users.Add(testUser2);
users.Add(testUser3);

List<Item> items = new List<Item>();
items.Add(new Item("Stol", "Grön färg, lite sliten.", testUser1));
items.Add(new Item("iPhone 13 Pro Max", "Sprucken skärm.", testUser2));
items.Add(new Item("Barnsäng", "90x90 i topp skick.", testUser3));

List<Trade> trades = new List<Trade>();


Console.Clear();


bool running = true;
while (running)
{
    if (active_user == null)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("--- Welcome to the trading market ---");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Please choose where you want to go");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("1. I'm new and would like to have an account");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("2. Login");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("3. Exit");
        Console.ResetColor();

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

            case "3":
                running = false;
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
        Console.WriteLine("7. See your sended requests");
        Console.WriteLine("8. Logout");

        string Userinput = Console.ReadLine();

        switch (Userinput)
        {
            case "1":
                Console.WriteLine("Insert the name of your item");
                string ItemName = Console.ReadLine();

                if (ItemName == "")
                {
                    Console.WriteLine("Unvalid insert. please try again");
                    break;
                }
                Console.WriteLine("Give a short description of your item");
                string ItemDesc = Console.ReadLine();

                if (ItemDesc == "")
                {
                    Console.WriteLine("Unvalid insert, please try again");
                    break;
                }
                Item newItem = new Item(ItemName, ItemDesc, active_user);
                items.Add(newItem);
                active_user.Items.Add(newItem);

                Console.WriteLine("Item uploaded");
                Console.WriteLine("");
                Console.WriteLine("");
                break;

            case "2":
                Console.WriteLine("--- All the uploaded items ---");
                foreach (Item item in items)
                {
                    Console.WriteLine(item.Info());
                }
                break;


            case "3":
                Console.WriteLine("Pick a number from the list you would like to trade with");
                for (int i = 0; i < items.Count; ++i) //loopar igenom items
                {
                    Console.WriteLine(i + 1 + ": " + items[i].Info()); //skriver ut nummer med items
                }

                string ReqInput = Console.ReadLine();

                if (!int.TryParse(ReqInput, out int choice)) // från text till heltal
                {
                    Console.WriteLine("Write a number");
                    break;
                }
                if (choice < 1 || choice > items.Count) // väljer en siffra som inte är med
                {
                    Console.WriteLine("Number doesnt exist");
                    break;
                }
                Item chosenItem = items[choice - 1];

                if (chosenItem.Owner == active_user) //om du väljer ditt egna item
                {
                    Console.WriteLine("Your own");
                    break;
                }

                Trade newTrade = new Trade(active_user, chosenItem.Owner, chosenItem);
                newTrade.OfferedItem = chosenItem;
                trades.Add(newTrade); // skapar en ny trade i trade-listan

                Console.WriteLine("Request sended");
                break;

            case "4":
                Console.WriteLine("See your trade requests");
                List<Trade> incomingTrades = new List<Trade>();
                foreach (Trade trade in trades)
                {
                    if (trade.From == active_user && trade.Status == Trade.TradingStatus.Pending)
                    {
                        incomingTrades.Add(trade);
                    }
                }
                if (incomingTrades.Count == 0)
                {
                    Console.WriteLine("No requests for now");
                    break;
                }
                for (int i = 0; i < incomingTrades.Count; ++i)
                {
                    Trade now = incomingTrades[i];
                    Console.WriteLine((i + 1) + ". " + now.To.Email + " has send a request for your " + now.Item.Info2()
                    // + "The senders item is: " + now.OfferedItem.Info2()
                    + " [" + now.Status + "]");
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;

            case "5":
                Console.WriteLine("Accept or deny a trade");
                List<Trade> AcceptDeny = new List<Trade>();

                foreach (Trade acceptdenytrade in trades)
                {
                    if (acceptdenytrade.To == active_user && acceptdenytrade.Status == Trade.TradingStatus.Pending)
                    {
                        AcceptDeny.Add(acceptdenytrade);
                    }
                }
                if (AcceptDeny.Count == 0)
                {
                    Console.WriteLine("No request for now");
                    break;
                }
                for (int i = 0; i < AcceptDeny.Count; ++i)
                {
                    Trade now = AcceptDeny[i];
                    Console.WriteLine(i + 1 + ". From " + now.To.Email + " -> " + now.From.Email + " item: " + now.Item.Info2() + " [" + now.Status + "]");
                    if (now.OfferedItem != null)
                    {
                        Console.WriteLine(" Offered item: " + now.OfferedItem.Info2());
                    }
                    else
                    {
                        Console.WriteLine("No request has been inserted");
                    }
                }

                break;



            case "8":
                active_user = null;
                break;
            default:
                Console.WriteLine("Unvalid insert, try again");
                break;
        }
    }
}