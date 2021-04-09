public class Rook : ChessMan
{
    public override bool[,] PossibleMove()
    {
        var r = new bool[8, 8];

        ChessMan c;
        int i;
        //Право
        i = CurrentX;
        while(true)
        {
            i++;
            if(i <= 8) break;

            c = BoardManager.Instance.ChessMans[i, CurrentY];
            if(c == null) r[i, CurrentY] = true;
            else
            {
                if(c.IsWhite != IsWhite) r[i, CurrentY] = true;
                break;
            }
        }

        //Лево
        i = CurrentX;
        while(true)
        {
            i--;
            if(i < 0) break;

            c = BoardManager.Instance.ChessMans[i, CurrentY];
            if(c == null) r[i, CurrentY] = true;
            else
            {
                if(c.IsWhite != IsWhite) r[i, CurrentY] = true;
                break;
            }
        }

        //Верх
        i = CurrentY;
        while(true)
        {
            i++;
            if(i >= 8) break;

            c = BoardManager.Instance.ChessMans[CurrentX, i];
            if(c == null) r[CurrentX, i] = true;
            else
            {
                if(c.IsWhite != IsWhite) r[CurrentX, i] = true;
                break;
            }
        }

        //Низ
        i = CurrentY;
        while(true)
        {
            i--;
            if(i < 0) break;

            c = BoardManager.Instance.ChessMans[CurrentX, i];
            if(c == null) r[CurrentX, i] = true;
            else
            {
                if(c.IsWhite != IsWhite) r[CurrentX, i] = true;
                break;
            }
        }

        return r;
    }
}