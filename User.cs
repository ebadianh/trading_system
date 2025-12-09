namespace TradingApp;

class User
{
    public int UserID;
    public string Email;
    public string User_Password;
    public List<Item> Items;


    public User(string email, string password)
    {
        Email = email;
        User_Password = password;
        Items = new List<Item>();
    }



    public bool TryLogin(string username, string password)
    {
        return username == Email && password == User_Password;
    }

    public string NewUser()
    {
        return Email;
    }
}