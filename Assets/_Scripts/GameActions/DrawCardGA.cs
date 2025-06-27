public class DrawCardGA : GameAction
{
    public DrawCardGA(int count)
    {
        Amount = count;
    }

    public int Amount { get; set; }
}