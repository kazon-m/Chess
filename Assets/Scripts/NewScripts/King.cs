public class King : ChessMan
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        ChessMan c;
        int i, j;
        //Верх
        i = CurrentX - 1;
        j = CurrentY + 1;
        if(CurrentY != 7)
        {
            for(var k = 0; k < 3; k++)
            {
                if(i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.ChessMans[i, j];
                    if(c == null) r[i, j] = true;
                    else if(IsWhite != c.IsWhite) r[i, j] = true;
                }

                i++;
            }
        }

        //Низ
        i = CurrentX - 1;
        j = CurrentY - 1;
        if(CurrentY != 0)
        {
            for(var k = 0; k < 3; k++)
            {
                if(i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.ChessMans[i, j];
                    if(c == null) r[i, j] = true;
                    else if(IsWhite != c.IsWhite) r[i, j] = true;
                }

                i++;
            }
        }

        //Лево
        if(CurrentX != 0)
        {
            c = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY];
            if(c == null) r[CurrentX - 1, CurrentY] = true;
            else if(IsWhite != c.IsWhite) r[CurrentX - 1, CurrentY] = true;
        }

        //Право
        if(CurrentX != 7)
        {
            c = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY];
            if(c == null) r[CurrentX + 1, CurrentY] = true;
            else if(IsWhite != c.IsWhite) r[CurrentX + 1, CurrentY] = true;
        }

        return r;
    }
}