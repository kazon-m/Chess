public class Pawn : ChessMan
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];
        ChessMan c, c2;

        if(IsWhite)
        {
            if(CurrentX != 0 && CurrentY != 7)
            {
                c = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY + 1];
                if(c != null && !c.IsWhite) r[CurrentX - 1, CurrentY + 1] = true;
            }

            if(CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY + 1];
                if(c != null && !c.IsWhite) r[CurrentX + 1, CurrentY + 1] = true;
            }

            if(CurrentY != 7)
            {
                c = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 1];
                if(c == null) r[CurrentX, CurrentY + 1] = true;
            }

            if(CurrentY == 1)
            {
                c = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 2];
                if(c == null && c2 == null) r[CurrentX, CurrentY + 2] = true;
            }
        }
        else
        {
            if(CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY - 1];
                if(c != null && c.IsWhite) r[CurrentX - 1, CurrentY - 1] = true;
            }

            if(CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY - 1];
                if(c != null && c.IsWhite) r[CurrentX + 1, CurrentY - 1] = true;
            }

            if(CurrentY != 0)
            {
                c = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 1];
                if(c == null) r[CurrentX, CurrentY - 1] = true;
            }

            if(CurrentY == 6)
            {
                c = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 2];
                if(c == null && c2 == null) r[CurrentX, CurrentY - 2] = true;
            }
        }

        return r;
    }
}