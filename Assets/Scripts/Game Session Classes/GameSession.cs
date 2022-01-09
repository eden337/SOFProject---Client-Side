public abstract class GameSession
{
    public int amountOfMazes = 0;
    public float avgTime = 0;
    public int mazeSizeRange = 4;

/// <summary>
/// The abstract game session class
/// </summary>
/// <param name="amountOfMazes">The amount of mazes in the level</param>
/// <param name="avgTime">The Average time of the level, its saved after the completion of the session</param>
/// <param name="mazeSizeRange">The size of the mazes in the session</param>
    public GameSession(int amountOfMazes,float avgTime, int mazeSizeRange){
        this.amountOfMazes=amountOfMazes;
        this.avgTime=avgTime;
        this.mazeSizeRange=mazeSizeRange;
    }

    public int AmountOfMazes
    {
        get => amountOfMazes;
        set
        {
            amountOfMazes = value;
        }
    }

    public float AvgTime
    {
        get => avgTime;
        set
        {
            avgTime = value;
        }
    }

    public int MazeSizeRange
    {
        get => mazeSizeRange;
        set
        {
            mazeSizeRange = value;
        }
    }


    public override string ToString()
    {
        return "Abstract Game Session";
    }

}
