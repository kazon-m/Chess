public class Bishop : ChessMan
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        ChessMan c;
        int i, j;

        //Верх лево
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i--;
            j++;
            if(i < 0 || j >= 8) break;
            c = BoardManager.Instance.ChessMans[i, j];
            if(c == null) r[i, j] = true;
            else
            {
                if(IsWhite != c.IsWhite) r[i, j] = true;
                break;
            }
        }

        //Верх право
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i++;
            j++;
            if(i >= 8 || j >= 8) break;
            c = BoardManager.Instance.ChessMans[i, j];
            if(c == null) r[i, j] = true;
            else
            {
                if(IsWhite != c.IsWhite) r[i, j] = true;
                break;
            }
        }

        //Низ лево
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i--;
            j--;
            if(i < 0 || j < 0) break;
            c = BoardManager.Instance.ChessMans[i, j];
            if(c == null) r[i, j] = true;
            else
            {
                if(IsWhite != c.IsWhite) r[i, j] = true;
                break;
            }
        }

        //Низ право
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i++;
            j--;
            if(i >= 8 || j < 0) break;
            c = BoardManager.Instance.ChessMans[i, j];
            if(c == null) r[i, j] = true;
            else
            {
                if(IsWhite != c.IsWhite) r[i, j] = true;
                break;
            }
        }

        return r;
    }
}