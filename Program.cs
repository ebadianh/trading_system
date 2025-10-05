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
        Console.WriteLine("═════╡ Welcome to the trading market ╞═════");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("1. Create an account");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("2. Login");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("3. Exit");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("Please choose your option: ");
        Console.ResetColor();

        string menuinput = Console.ReadLine();
        switch (menuinput)
        {
            case "1":
                Console.WriteLine("Enter your username:");
                string newUsername = Console.ReadLine();

                Console.WriteLine("And your password:");
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
                    Console.WriteLine("Username is already taken.");
                }
                else
                {
                    users.Add(new User(newUsername, newPassword));
                    Console.WriteLine("The account has been created!");
                }
                break;

            case "2":
                if (active_user == null)
                {
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();

                    Console.WriteLine("Password:");
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
        int waiting = 0;
        foreach (Trade trade in trades)
        {
            if (trade.To.Email == active_user.Email && trade.Status == Trade.TradingStatus.Pending)
            {
                waiting++;
            }
        }
        Console.WriteLine("═════╡ Welcome dear " + active_user.Email + " ╞═════");
        Console.WriteLine("══════════════════════════════════");
        Console.WriteLine("1. Upload an item");
        Console.WriteLine("2. Browse through all items");
        Console.WriteLine("3. Send a request for another user's item");
        Console.WriteLine("4. See your sent trade requests");
        Console.WriteLine("5. Accept or deny a request (" + waiting + ")");
        Console.WriteLine("6. Browse through completed trades");
        Console.WriteLine("7. Logout");
        Console.WriteLine("8. Exit");
        Console.Write("Please choose your option: ");

        string Userinput = Console.ReadLine();

        switch (Userinput)
        {
            case "1":
                Console.WriteLine("Insert the name of your item");
                string ItemName = Console.ReadLine();

                if (ItemName == "")
                {
                    Console.WriteLine("Invalid input, please try again");
                    break;
                }
                Console.WriteLine("Give a short description of your item");
                string ItemDesc = Console.ReadLine();

                if (ItemDesc == "")
                {
                    Console.WriteLine("Invalid input, please try again");
                    break;
                }
                Item newItem = new Item(ItemName, ItemDesc, active_user);
                items.Add(newItem);
                active_user.Items.Add(newItem);

                Console.WriteLine("Item has been successfully uploaded");
                Console.WriteLine();
                break;

            case "2":
                Console.WriteLine("═════╡ All the uploaded items ╞═════");
                foreach (Item item in items)
                {
                    Console.WriteLine(item.Info());
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;


            case "3":
                Console.WriteLine("═════╡ Choose an item you like to trade for ╞═════");
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
                    Console.WriteLine("Sorry, you can't choose your own item");
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

                Console.WriteLine("Request has been sent");
                break;

            case "4":
                Console.WriteLine("═════╡ See your sent trade requests ╞═════");
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
                    Console.WriteLine("You haven't sent any requests yet");
                    Console.WriteLine();
                    break;
                }
                for (int i = 0; i < sentTrades.Count; ++i)
                {
                    Trade trade = sentTrades[i];
                    Console.WriteLine("═══════════════════════════════════════════");
                    Console.WriteLine("Request number: " + (i + 1));
                    Console.WriteLine("To: " + trade.To.Email);
                    Console.WriteLine("Requested item: " + trade.RequestedItem.Info2());

                    if (trade.OfferedItem != null)
                    {
                        Console.WriteLine("You offered: " + trade.OfferedItem.Info2());
                    }
                    else
                    {
                        Console.WriteLine("No item has been offered");
                        Console.WriteLine();
                    }
                    Console.WriteLine("Status: " + trade.Status);
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;

            case "5":
                Console.WriteLine("═════╡ List of trades waiting for a decision ╞═════");
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
                    Console.WriteLine("You have no trades waiting for a decision");
                    break;
                }

                for (int i = 0; i < tradesinPending.Count; ++i)
                {
                    Trade trade = tradesinPending[i];
                    Console.WriteLine("═══════════════════════════════════");
                    Console.WriteLine("Request number: " + (i + 1));
                    Console.WriteLine("From: " + trade.From.Email);
                    Console.WriteLine("Requested item: " + trade.RequestedItem.Info2());
                    Console.WriteLine("Offered item: " + trade.OfferedItem.Info2());
                    Console.WriteLine("Status: " + trade.Status);
                }


                Console.WriteLine("═══════════════════════════════════");
                Console.WriteLine("Enter the number of the request you want to respond to:");

                string responseInput = Console.ReadLine();
                int responseChoice;

                if (!int.TryParse(responseInput, out responseChoice) || responseChoice < 1 || responseChoice > tradesinPending.Count)
                {
                    Console.WriteLine("Invalid choice");
                    break;
                }

                Trade selectedTrade = tradesinPending[responseChoice - 1];

                Console.WriteLine("Please make your choice (y = yes, n = no)");
                string decision = Console.ReadLine();

                if (decision == "y" || decision == "Y")
                {
                    selectedTrade.RequestedItem.Owner.Items.Remove(selectedTrade.RequestedItem);
                    selectedTrade.From.Items.Add(selectedTrade.RequestedItem);
                    selectedTrade.RequestedItem.Owner = selectedTrade.From;

                    selectedTrade.From.Items.Remove(selectedTrade.OfferedItem);
                    active_user.Items.Add(selectedTrade.OfferedItem);
                    selectedTrade.OfferedItem.Owner = active_user;

                    selectedTrade.Status = Trade.TradingStatus.Accepted;
                    Console.WriteLine("Trade has been accepted");
                }
                else if (decision == "n" || decision == "N")
                {
                    selectedTrade.Status = Trade.TradingStatus.Denied;
                    Console.WriteLine("Trade has been denied");
                }
                else
                {
                    Console.WriteLine("Invalid input. Trade remains pending");
                }
                break;

            case "6":
                Console.WriteLine("═════╡ All of the completed trades ╞═════");
                List<Trade> completedTrades = new List<Trade>();
                foreach (Trade trade in trades)
                {
                    if (trade.Status == Trade.TradingStatus.Accepted || trade.Status == Trade.TradingStatus.Denied)
                    {
                        completedTrades.Add(trade);
                    }
                }

                if (completedTrades.Count == 0)
                {
                    Console.WriteLine("No trades have been completed yet");
                    break;
                }

                for (int i = 0; i < completedTrades.Count; ++i)
                {
                    Trade trade = completedTrades[i];
                    Console.WriteLine("═══════════════════════════════════");
                    Console.WriteLine("Trade number: " + (i + 1));
                    Console.WriteLine("From: " + trade.From.Email);
                    Console.WriteLine("To: " + trade.To.Email);
                    Console.WriteLine("Requested item: " + trade.RequestedItem.Info2());
                    Console.WriteLine("Offered item: " + trade.OfferedItem.Info2());
                    Console.WriteLine("Status: " + Trade.TradingStatus.Completed);
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to go back...");
                Console.ReadLine();
                break;

            case "7":
                active_user = null;
                break;

            case "8":
                running = false;
                break;

            default:
                Console.WriteLine("Invalid input, please try again");
                break;
        }
    }
}