/// <summary>
/// This class holds the info of an edge in the maze graph, such has the nodes it connected to, 
/// the weight of the edge (as the distance), 
/// how many times we passed this edge and if the edge is vertical or horizontal.
/// </summary>
public class MazeEdgeInfo
{
    #region Properties
    private int distance;
    private int passed;
    private int node1;
    private int node2;
    private bool vertical;
    #endregion

    #region Constructor
    public MazeEdgeInfo(int distance, int node1, int node2)
    {
        this.distance = distance;
        this.node1 = node1;
        this.node2 = node2;
        passed = 0;

    }
    #endregion

    #region Methods
    
    public void setVertical(bool vertical)
    {
        this.vertical = vertical;
    }

    public int getDistance()
    {
        return distance;
    }
    public int getPassed()
    {
        return passed;
    }
    public int addPassed()
    {
        passed++;
        return passed;
    }
    public void resetPass()
    {
        passed = 0;
    }

    public override bool Equals(object obj)
    {
        return obj is MazeEdgeInfo info &&
               node1 == info.node1 &&
               node2 == info.node2;
    }

    public override int GetHashCode()
    {
        int hashCode = -1413141567;
        hashCode = hashCode * -1521134295 + node1.GetHashCode();
        hashCode = hashCode * -1521134295 + node2.GetHashCode();
        return hashCode;
    }

    #endregion

}