using UnityEngine;

public abstract class ChessMan : MonoBehaviour
{
    public bool IsWhite;
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove() => new bool[8, 8];
}