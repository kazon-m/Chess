public class Horse : ChessMan
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        //Лево
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);

        //Право
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);

        //Право верх
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        //Право низ
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        //Лево низ
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);

        //низ Право
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);

        //Лево верх
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        //Лево низ
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);
        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        ChessMan c;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.ChessMans[x, y];
            if(c == null) r[x, y] = true;
            else if(IsWhite != c.IsWhite) r[x, y] = true;
        }
    }
}