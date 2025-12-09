using MySql.Data.MySqlClient;
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
                var user = new User(reader["email"].ToString() ?? "", reader["user_password"].ToString() ?? "");
                user.UserID = Convert.ToInt32(reader["UserID"]);
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


}