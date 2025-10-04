using System.Linq.Expressions;
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

testUser1.Items.Add(items[0]);
testUser2.Items.Add(items[1]);
testUser3.Items.Add(items[2]);

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
        Console.WriteLine("--- Welcome dear " + active_user.Email + " ---");
        Console.WriteLine();
        Console.WriteLine("1. Upload item");
        Console.WriteLine("2. Browse trough items");
        Console.WriteLine("3. Request a trade from another");
        Console.WriteLine("4. See your sent trade requests");
        Console.WriteLine("5. Accept or deny a request");
        Console.WriteLine("6. Browse trough completed trades");
        Console.WriteLine("7. Logout");

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

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;


            case "3":
                Console.WriteLine("Choose a item you like to have");
                for (int i = 0; i < items.Count; ++i) //loopar igenom items
                {
                    Console.WriteLine(i + 1 + ": " + items[i].Info()); //skriver ut nummer med items
                }

                string RequestInput = Console.ReadLine();
                int RequestChoice;

                if (!int.TryParse(RequestInput, out RequestChoice) || RequestChoice < 1 || RequestChoice > items.Count)  // från text till heltal
                {
                    Console.WriteLine("Invalid choice");
                    break;
                }

                Item requesteditem = items[RequestChoice - 1];

                if (requesteditem.Owner == active_user) //om du väljer ditt egna item
                {
                    Console.WriteLine("Sorry, your own item");
                    break;
                }

                if (active_user.Items.Count == 0)
                {
                    Console.WriteLine("You have no items to offer");
                    break;
                }


                Console.WriteLine("Now choose one of your own items you like to offer");
                for (int i = 0; i < active_user.Items.Count; ++i)
                {
                    Console.WriteLine((i + 1) + ": " + active_user.Items[i].Info2());
                }
                string offerInput = Console.ReadLine();
                int offerChoice;

                if (!int.TryParse(offerInput, out offerChoice) || offerChoice < 1 || offerChoice > active_user.Items.Count)
                {
                    Console.WriteLine("Invalid choice");
                }

                Item offereditem = active_user.Items[offerChoice - 1];

                Trade newTrade = new Trade(active_user, requesteditem.Owner, requesteditem, offereditem);
                trades.Add(newTrade); // skapar en ny trade i trade-listan

                Console.WriteLine("Request sent");
                break;

            case "4":
                Console.WriteLine("See your sent trade requests");
                List<Trade> sentTrades = new List<Trade>();

                foreach (Trade trade in trades)
                {
                    if (trade.From.Email == active_user.Email && trade.Status == Trade.TradingStatus.Pending)
                    {
                        sentTrades.Add(trade);
                    }
                }
                if (sentTrades.Count == 0)
                {
                    Console.WriteLine("You havent sent any request yet");
                    break;
                }
                for (int i = 0; i < sentTrades.Count; ++i)
                {
                    Trade trade = sentTrades[i];
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Request number: " + (i + 1));
                    Console.WriteLine("To: " + trade.To.Email);
                    Console.WriteLine("Requested item: " + trade.RequestedItem.Info2());

                    if (trade.OfferedItem != null)
                    {
                        Console.WriteLine("You offered: " + trade.OfferedItem.Info2());
                    }
                    else
                    {
                        Console.WriteLine("No item offered");
                    }
                    Console.WriteLine("Status: " + trade.Status);
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;

            case "5":
                Console.WriteLine("List of trades waiting for decision");
                List<Trade> tradesinPending = new List<Trade>();
                foreach (Trade trade in trades)
                {
                    if (trade.To.Email == active_user.Email && trade.Status == Trade.TradingStatus.Pending)
                    {
                        tradesinPending.Add(trade);
                    }
                }
                if (tradesinPending.Count == 0)
                {
                    Console.WriteLine("You have no trades waiting for decision");
                    break;
                }

                for (int i = 0; i < tradesinPending.Count; ++i)
                {
                    Trade trade = tradesinPending[i];
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Request number: " + (i + 1));
                    Console.WriteLine("From: " + trade.From.Email);
                    Console.WriteLine("They want: " + trade.RequestedItem.Info2());
                    Console.WriteLine("They offer: " + trade.OfferedItem.Info2());
                    Console.WriteLine("Status: " + trade.Status);
                }


                Console.WriteLine("-------------------");
                Console.WriteLine("Enter the number of the request you want to respond to: ");

                string responseInput = Console.ReadLine();
                int responseChoice;

                if (!int.TryParse(responseInput, out responseChoice) || responseChoice < 1 || responseChoice > tradesinPending.Count)
                {
                    Console.WriteLine("Invalid choice");
                    break;
                }

                Trade selectedTrade = tradesinPending[responseChoice - 1];

                Console.WriteLine("Enter y for yes or n for no");
                string decision = Console.ReadLine();

                if (decision == "y" || decision == "Y")
                {
                    Console.WriteLine("Trade has been accepted");
                }
                else if (decision == "n" || decision == "N")
                {
                    Console.WriteLine("Trade has been denied");
                }
                else
                {
                    Console.WriteLine("Invalid input. Trade remains pending");
                }
                break;





            case "7":
                active_user = null;
                break;
            default:
                Console.WriteLine("Unvalid insert, try again");
                break;
        }
    }
}