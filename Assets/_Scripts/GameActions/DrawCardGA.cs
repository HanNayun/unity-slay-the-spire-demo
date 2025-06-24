public class DrawCardGA : GameAction
{
    public int Amount { get; set; }

    public void DrawCard(int count)
    {
        Amount = count;
    }
}