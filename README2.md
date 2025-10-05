Vilken funktion/metod jag valt.

Varför har jag valt den?

-------------------------------

1. Jag valde först att skapa en interface IUser men insåg väldigt snabbt att jag behöver inte jobba med kontrakt då mitt system inte är väldigt komplext. Hade jag haft flera olika typer av användare så hade jag använt mig interfaces. Istället valde jag komposition för att jag väldigt enkelt enbart ska ha klasser som Trade, User, Item. 
Varje klass har olika egenskaper som integreras med varandra i mitt Program.cs.

class User = här har varje användare en lista med sina egna objekt samt att en användare kan ha flera objekt. 

class Item = När ett objekt skapas kopplas det till en användare och genom User Owner får varje item en ägare.

class Trade = här kopplar jag två användare och två objekt. Slutligen använder jag mig av enum i denna klass för att kunna sätta en status på byten som befinner sig i olika moment.

2. All kod i mitt Program.cs körs genom en boolean för att jag vill hålla igång loopen om användaren själv inte väljer att avsluta. En boolean är perfekt för en ja-och nej-logik för det räcker med true och false utan att användaren behöver krånga till det med siffror och texter. Särskilt när jag använder mig av menyer som jag då skapat genom while-loop och switch. 

3. Jag valde att jobba med List<T> istället för Arrays[] eftersom jag ville skapa listor som växer dynamiskt. Om jag skulle använt Arrays[] så hade jag behövt ha ett förbestämt antal platser. Däremot använder jag mig av Arrays under huven på List<T> eftersom jag vill numerera användarna, items och 

4. Jag har även använt mig av foreach och for för att gå igenom listor vid trades, users och items och med hjälp av loopen kan jag använda information för att bestämma vad som skall hända.

5. Slutligen när jag kom till den allra sista delen av programmet där jag valde att arbeta med att items vid ett completed trade där en user har godkänt ett byte så byts ägarna. Det var det mest tillfredställande för allting flöt bara på när jag skrev ut varje kodrad. Exempel:

- selectedTrade.RequestedItem.Owner.Items.Remove(selectedTrade.RequestedItem);
- selectedTrade.From.Items.Add(selectedTrade.RequestedItem);
- selectedTrade.RequestedItem.Owner = selectedTrade.From;

- selectedTrade.From.Items.Remove(selectedTrade.OfferedItem);
- active_user.Items.Add(selectedTrade.OfferedItem);
- selectedTrade.OfferedItem.Owner = active_user;

- selectedTrade.Status = Trade.TradingStatus.Accepted;
- Console.WriteLine("Trade has been accepted");

Jag förstår verkligen logiken bakom hela det kodblocket. Trots att jag vet att jag fortfarande har en lång väg att gå så tycker jag det känns roligt, spännande och fortfarande väldigt utmanande.