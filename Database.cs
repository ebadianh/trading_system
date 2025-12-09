using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using TradingApp;

class Database
{
    private string db = "Server=localhost;Database=trading_system_db;User ID=root;Password=BeLLe;";

    public void SaveUser(User user)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = "INSERT INTO users (email, user_password) VALUES (@email, @user_password)";
            MySqlCommand sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@email", user.Email);
            sqlcmd.Parameters.AddWithValue("@user_password", user.User_Password);
            sqlcmd.ExecuteNonQuery();

            try { user.UserID = (int)sqlcmd.LastInsertedId; } catch { }

            Console.WriteLine("Användare sparad i databasen!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid databasinsättning: " + error.Message);
        }
    }
    public User? LoginUser(string email, string password)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = "SELECT * FROM users WHERE email = @email AND user_password = @password";
            MySqlCommand sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@email", email);
            sqlcmd.Parameters.AddWithValue("@password", password);
            using var reader = sqlcmd.ExecuteReader();

            if (reader.Read())
            {
                var user = new User(reader["Email"].ToString() ?? "", reader["User_Password"].ToString() ?? "")
                {
                    UserID = Convert.ToInt32(reader["UserID"])
                };
                user.Items = GetItemsByUser(user);
                return user;
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid inloggning: " + error.Message);
        }

        return null;
    }

    public void SaveItem(Item item)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = "INSERT INTO items (ItemName, Description, OwnerID) VALUES (@name, @desc, @ownerId)";
            var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@name", item.ItemName);
            sqlcmd.Parameters.AddWithValue("@desc", item.Description);
            sqlcmd.Parameters.AddWithValue("@ownerId", item.Owner.UserID);
            sqlcmd.ExecuteNonQuery();

            try { item.ItemID = (int)sqlcmd.LastInsertedId; } catch { }

            Console.WriteLine("Item sparad i databasen!");
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid insättning av item: " + error.Message);
        }
    }

    public List<Item> GetItemsByUser(User user)
    {
        var items = new List<Item>();
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = "SELECT ItemID, ItemName, Description FROM items WHERE OwnerID = @userId";
            var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@userId", user.UserID);

            using var reader = sqlcmd.ExecuteReader();
            while (reader.Read())
            {
                var item = new Item(
                    reader["ItemName"].ToString() ?? "",
                    reader["Description"].ToString() ?? "",
                    user
                );
                item.ItemID = Convert.ToInt32(reader["ItemID"]);
                items.Add(item);
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid hämtning av items: " + error.Message);
        }

        return items;
    }
    public List<Item> GetAllItems()
    {
        var items = new List<Item>();
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"SELECT i.ItemID, i.ItemName, i.Description,
                                u.UserID, u.Email, u.User_Password
                         FROM items i
                         JOIN users u ON i.OwnerID = u.UserID";
            using var sqlcmd = new MySqlCommand(query, connection);
            using var reader = sqlcmd.ExecuteReader();

            while (reader.Read())
            {
                var email = reader["Email"].ToString() ?? "";
                var password = reader["User_Password"].ToString() ?? "";
                var owner = new User(email, password)
                {
                    UserID = Convert.ToInt32(reader["UserID"])
                };

                var itemName = reader["ItemName"].ToString() ?? "";
                var description = reader["Description"].ToString() ?? "";

                var item = new Item(itemName, description, owner)
                {
                    ItemID = Convert.ToInt32(reader["ItemID"])
                };

                items.Add(item);
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid hämtning av alla items: " + error.Message);
        }

        return items;
    }
    public void SaveTrade(Trade trade)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"INSERT INTO trades 
                         (FromUserID, ToUserID, RequestedItemID, OfferedItemID, Status)
                         VALUES (@fromId, @toId, @requestedId, @offeredId, @status)";
            using var sqlcmd = new MySqlCommand(query, connection);

            sqlcmd.Parameters.AddWithValue("@fromId", trade.From.UserID);
            sqlcmd.Parameters.AddWithValue("@toId", trade.To.UserID);
            sqlcmd.Parameters.AddWithValue("@requestedId", trade.RequestedItem.ItemID);

            if (trade.OfferedItem != null)
                sqlcmd.Parameters.AddWithValue("@offeredId", trade.OfferedItem.ItemID);
            else
                sqlcmd.Parameters.AddWithValue("@offeredId", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@status", trade.Status.ToString());

            sqlcmd.ExecuteNonQuery();

            trade.TradeID = (int)sqlcmd.LastInsertedId;
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid sparande av trade: " + error.Message);
        }
    }
    public List<Trade> GetReceivedTrades(User user)
    {
        var trades = new List<Trade>();
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"SELECT t.TradeID, t.Status,
                                f.UserID AS FromUserID, f.Email AS FromEmail,
                                ri.ItemID AS RequestedItemID, ri.ItemName AS RequestedItemName, ri.Description AS RequestedItemDesc,
                                oi.ItemID AS OfferedItemID, oi.ItemName AS OfferedItemName, oi.Description AS OfferedItemDesc
                         FROM trades t
                         JOIN users f ON t.FromUserID = f.UserID
                         JOIN items ri ON t.RequestedItemID = ri.ItemID
                         LEFT JOIN items oi ON t.OfferedItemID = oi.ItemID
                         WHERE t.ToUserID = @userId AND t.Status = 'Pending'";
            using var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@userId", user.UserID);

            using var reader = sqlcmd.ExecuteReader();
            while (reader.Read())
            {
                var fromUser = new User(reader["FromEmail"].ToString() ?? "", "")
                {
                    UserID = Convert.ToInt32(reader["FromUserID"])
                };

                var requestedItem = new Item(reader["RequestedItemName"].ToString() ?? "",
                                             reader["RequestedItemDesc"].ToString() ?? "",
                                             user)
                {
                    ItemID = Convert.ToInt32(reader["RequestedItemID"])
                };

                Item? offeredItem = null;
                if (reader["OfferedItemID"] != DBNull.Value)
                {
                    offeredItem = new Item(reader["OfferedItemName"].ToString() ?? "",
                                           reader["OfferedItemDesc"].ToString() ?? "",
                                           fromUser)
                    {
                        ItemID = Convert.ToInt32(reader["OfferedItemID"])
                    };
                }

                var status = Enum.Parse<Trade.TradingStatus>(reader["Status"].ToString() ?? "Pending");

                trades.Add(new Trade(Convert.ToInt32(reader["TradeID"]), fromUser, user, requestedItem, offeredItem, status));
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid hämtning av mottagna trades: " + error.Message);
        }

        return trades;
    }
    public List<Trade> GetSentTrades(User user)
    {
        var trades = new List<Trade>();
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"SELECT t.TradeID, t.FromUserID, t.ToUserID, t.RequestedItemID, t.OfferedItemID, t.Status,
                               u_from.Email as FromEmail, u_from.User_Password,
                               u_to.Email as ToEmail, u_to.User_Password as ToPassword,
                               i_req.ItemName as ReqItemName, i_req.Description as ReqItemDesc, i_req.OwnerID as ReqOwnerID,
                               i_off.ItemName as OffItemName, i_off.Description as OffItemDesc, i_off.OwnerID as OffOwnerID
                        FROM trades t
                        JOIN users u_from ON t.FromUserID = u_from.UserID
                        JOIN users u_to ON t.ToUserID = u_to.UserID
                        JOIN items i_req ON t.RequestedItemID = i_req.ItemID
                        LEFT JOIN items i_off ON t.OfferedItemID = i_off.ItemID
                        WHERE t.FromUserID = @userId";

            using var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@userId", user.UserID);
            using var reader = sqlcmd.ExecuteReader();

            while (reader.Read())
            {
                User fromUser = new User(reader["FromEmail"].ToString() ?? "", reader["User_Password"].ToString() ?? "")
                {
                    UserID = Convert.ToInt32(reader["FromUserID"])
                };

                User toUser = new User(reader["ToEmail"].ToString() ?? "", reader["ToPassword"].ToString() ?? "")
                {
                    UserID = Convert.ToInt32(reader["ToUserID"])
                };

                Item requestedItem = new Item(reader["ReqItemName"].ToString() ?? "", reader["ReqItemDesc"].ToString() ?? "", fromUser)
                {
                    ItemID = Convert.ToInt32(reader["RequestedItemID"])
                };

                Item? offeredItem = null;
                if (reader["OfferedItemID"] != DBNull.Value)
                {
                    offeredItem = new Item(reader["OffItemName"].ToString() ?? "", reader["OffItemDesc"].ToString() ?? "", toUser)
                    {
                        ItemID = Convert.ToInt32(reader["OfferedItemID"])
                    };
                }

                Trade trade = new Trade(
                    Convert.ToInt32(reader["TradeID"]),
                    fromUser,
                    toUser,
                    requestedItem,
                    offeredItem,
                    Enum.Parse<Trade.TradingStatus>(reader["Status"].ToString() ?? "Pending")
                );

                trades.Add(trade);
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid hämtning av skickade trades: " + error.Message);
        }

        return trades;
    }
    public void UpdateTradeStatus(int tradeId, Trade.TradingStatus status)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"UPDATE trades SET Status = @status WHERE TradeID = @tradeId";
            using var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@status", status.ToString());
            sqlcmd.Parameters.AddWithValue("@tradeId", tradeId);

            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid uppdatering av trade-status: " + error.Message);
        }
    }

    public void UpdateItemOwner(int itemId, int newOwnerId)
    {
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = "UPDATE items SET OwnerID = @ownerId WHERE ItemID = @itemId";
            using var sqlcmd = new MySqlCommand(query, connection);
            sqlcmd.Parameters.AddWithValue("@ownerId", newOwnerId);
            sqlcmd.Parameters.AddWithValue("@itemId", itemId);
            sqlcmd.ExecuteNonQuery();
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid uppdatering av item-owner: " + error.Message);
        }
    }

    public List<Trade> GetAllCompletedTrades()
    {
        var trades = new List<Trade>();
        using var connection = new MySqlConnection(db);
        try
        {
            connection.Open();
            string query = @"SELECT t.TradeID, t.Status,
                                    t.FromUserID, f.Email AS FromEmail,
                                    t.ToUserID, to_u.Email AS ToEmail,
                                    ri.ItemID AS RequestedItemID, ri.ItemName AS RequestedItemName, ri.Description AS RequestedItemDesc,
                                    oi.ItemID AS OfferedItemID, oi.ItemName AS OfferedItemName, oi.Description AS OfferedItemDesc
                             FROM trades t
                             JOIN users f ON t.FromUserID = f.UserID
                             JOIN users to_u ON t.ToUserID = to_u.UserID
                             JOIN items ri ON t.RequestedItemID = ri.ItemID
                             LEFT JOIN items oi ON t.OfferedItemID = oi.ItemID
                             WHERE t.Status = 'Completed'";
            using var sqlcmd = new MySqlCommand(query, connection);
            using var reader = sqlcmd.ExecuteReader();

            while (reader.Read())
            {
                var fromUser = new User(reader["FromEmail"].ToString() ?? "", "")
                {
                    UserID = Convert.ToInt32(reader["FromUserID"])
                };

                var toUser = new User(reader["ToEmail"].ToString() ?? "", "")
                {
                    UserID = Convert.ToInt32(reader["ToUserID"])
                };

                var requestedItem = new Item(reader["RequestedItemName"].ToString() ?? "",
                                             reader["RequestedItemDesc"].ToString() ?? "",
                                             fromUser)
                {
                    ItemID = Convert.ToInt32(reader["RequestedItemID"])
                };

                Item? offeredItem = null;
                if (reader["OfferedItemID"] != DBNull.Value)
                {
                    offeredItem = new Item(reader["OfferedItemName"].ToString() ?? "",
                                           reader["OfferedItemDesc"].ToString() ?? "",
                                           toUser)
                    {
                        ItemID = Convert.ToInt32(reader["OfferedItemID"])
                    };
                }

                var status = Enum.Parse<Trade.TradingStatus>(reader["Status"].ToString() ?? "Completed");

                trades.Add(new Trade(Convert.ToInt32(reader["TradeID"]), fromUser, toUser, requestedItem, offeredItem, status));
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Fel vid hämtning av completed trades: " + error.Message);
        }

        return trades;
    }

}