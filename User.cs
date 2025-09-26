namespace TradingApp;

class User
{
    public string Email;
    private string _password;


    public User(string email, string password)
    {
        Email = email;
        _password = password;
    }

    public List<Item> item = new List<Item>();

    public bool TryLogin(string username, string password)
    {
        return username == Email && password == _password;
    }

    public string NewUser()
    {
        return Email;
    }



}