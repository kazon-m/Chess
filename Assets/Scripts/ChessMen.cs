using UnityEngine;

public class ChessMen : MonoBehaviour
{
    public int Type;         //Тип фигуры Пешка, Король и тд.
    public bool White;       //Цвет фигуры белый-черный
    public int LastStep;     //Последний ход фигуры
    public int Walked;       //Сколько ходов ходила фигура
    public bool IsACastling; //Есть ли возможность рокировки

    public int X;
    public int Y;

    public int MX;
    public int MY;

    public byte[,] ChessMove = new byte[8, 8];

    public static ChessMen Instance { get; set; }
    //public byte[,] ChessMovePrevious = new byte[8, 8];
    //public byte[,] ChessMoveNext = new byte[8, 8];
    // 0 - Хода нет
    // 1 - Есть ход
    // 2 - Может убить

    private void Awake() => Instance = this;

    public byte[,] PossibleMove()
    {
        IsACastling = false;
        ChessMove = new byte[8, 8];
        var chess = new ChessMen[2];
        switch(Type)
        {
            case 0: // Пешка
            {
                if(White)
                {
                    if(X != 0 && Y != 0) // Вперед влево
                    {
                        chess[0] = OnBoard.Instance.Board[X - 1, Y - 1].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
                        if(chess[0] != null && White != chess[0].White) ChessMove[X - 1, Y - 1] = 2;
                        else if(chess[1] != null
                             && White != chess[1].White
                             && chess[1].Walked == 1
                             && OnBoard.Instance.Step == chess[1].LastStep + 1) ChessMove[X - 1, Y - 1] = 2;
                    }

                    if(X != 0 && Y != 7) // Вперед вправо
                    {
                        chess[0] = OnBoard.Instance.Board[X - 1, Y + 1].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
                        if(chess[0] != null && White != chess[0].White) ChessMove[X - 1, Y + 1] = 2;
                        else if(chess[1] != null
                             && White != chess[1].White
                             && chess[1].Walked == 1
                             && OnBoard.Instance.Step == chess[1].LastStep + 1) ChessMove[X - 1, Y + 1] = 2;
                    }

                    if(X != 0 && X != 6) //Вперед
                    {
                        chess[0] = OnBoard.Instance.Board[X - 1, Y].GetComponentInChildren<ChessMen>();
                        if(chess[0] == null) ChessMove[X - 1, Y] = 1;
                    }
                    else if(X == 6) //Первый ход, два хода вперед
                    {
                        chess[0] = OnBoard.Instance.Board[X - 1, Y].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X - 2, Y].GetComponentInChildren<ChessMen>();
                        if(chess[0] == null) ChessMove[X - 1, Y] = 1;
                        if(chess[0] == null && chess[1] == null) ChessMove[X - 2, Y] = 1;
                    }
                }
                else
                {
                    if(X != 7 && Y != 0) // Вперед влево
                    {
                        chess[0] = OnBoard.Instance.Board[X + 1, Y - 1].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
                        if(chess[0] != null && White != chess[0].White) ChessMove[X + 1, Y - 1] = 2;
                        else if(chess[1] != null
                             && White != chess[1].White
                             && chess[1].Walked == 1
                             && OnBoard.Instance.Step == chess[1].LastStep + 1) ChessMove[X + 1, Y - 1] = 2;
                    }

                    if(X != 7 && Y != 7) // Вперед вправо
                    {
                        chess[0] = OnBoard.Instance.Board[X + 1, Y + 1].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
                        if(chess[0] != null && White != chess[0].White) ChessMove[X + 1, Y + 1] = 2;
                        else if(chess[1] != null
                             && White != chess[1].White
                             && chess[1].Walked == 1
                             && OnBoard.Instance.Step == chess[1].LastStep + 1) ChessMove[X + 1, Y + 1] = 2;
                    }

                    if(X != 7 && X != 1) //Вперед
                    {
                        chess[0] = OnBoard.Instance.Board[X + 1, Y].GetComponentInChildren<ChessMen>();
                        if(chess[0] == null) ChessMove[X + 1, Y] = 1;
                    }
                    else if(X == 1) //Первый ход, два хода вперед
                    {
                        chess[0] = OnBoard.Instance.Board[X + 1, Y].GetComponentInChildren<ChessMen>();
                        chess[1] = OnBoard.Instance.Board[X + 2, Y].GetComponentInChildren<ChessMen>();
                        if(chess[0] == null) ChessMove[X + 1, Y] = 1;
                        if(chess[0] == null && chess[1] == null) ChessMove[X + 2, Y] = 1;
                    }
                }

                break;
            }
            case 1: //Ладья
            {
                for(var i = Y + 1; i < 8; i++) //Право
                {
                    chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[X, i] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[X, i] = 2;
                        else
                        {
                            if(chess[0].Walked == 0 && chess[0].Type == 5)
                            {
                                ChessMove[X, i] = 1;
                                IsACastling = true;
                            }
                        }

                        break;
                    }
                }

                for(var i = Y - 1; i >= 0; i--) //Лево
                {
                    chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[X, i] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[X, i] = 2;
                        else
                        {
                            if(chess[0].Walked == 0 && chess[0].Type == 5)
                            {
                                ChessMove[X, i] = 1;
                                IsACastling = true;
                            }
                        }

                        break;
                    }
                }

                for(var i = X - 1; i >= 0; i--) //Верх
                {
                    chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, Y] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[i, Y] = 2;
                        break;
                    }
                }

                for(var i = X + 1; i < 8; i++) //Низ
                {
                    chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, Y] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[i, Y] = 2;
                        break;
                    }
                }

                break;
            }
            case 2: //Конь
            {
                //верх право 1
                KnightChessMove(X - 1, Y + 2, ref ChessMove);

                //Верх право 2
                KnightChessMove(X - 2, Y + 1, ref ChessMove);

                //Верх лево 1
                KnightChessMove(X - 2, Y - 1, ref ChessMove);

                //Верх лево 2
                KnightChessMove(X - 1, Y - 2, ref ChessMove);

                //низ право 1
                KnightChessMove(X + 2, Y + 1, ref ChessMove);

                //Низ право 2
                KnightChessMove(X + 1, Y + 2, ref ChessMove);

                //низ лево 1
                KnightChessMove(X + 1, Y - 2, ref ChessMove);

                //Низ лево 2
                KnightChessMove(X + 2, Y - 1, ref ChessMove);

                break;
            }
            case 3: //Слон
            {
                for(int i = X - 1, j = Y + 1; i >= 0 && j < 8; i--, j++) //Вперед вправо
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X - 1, j = Y - 1; i >= 0 && j >= 0; i--, j--) //Вперед влево
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X + 1, j = Y + 1; i < 8 && j < 8; i++, j++) //Вниз вправо
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X + 1, j = Y - 1; i < 8 && j >= 0; i++, j--) //Вниз влево
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                break;
            }
            case 4: //Королева
            {
                for(var i = Y + 1; i < 8; i++) //Право
                {
                    chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[X, i] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[X, i] = 2;
                        break;
                    }
                }

                for(var i = Y - 1; i >= 0; i--) //Лево
                {
                    chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[X, i] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[X, i] = 2;
                        break;
                    }
                }

                for(var i = X - 1; i >= 0; i--) //Верх
                {
                    chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, Y] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[i, Y] = 2;
                        break;
                    }
                }

                for(var i = X + 1; i < 8; i++) //Низ
                {
                    chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, Y] = 1;
                    else
                    {
                        if(chess[0].White != White) ChessMove[i, Y] = 2;
                        break;
                    }
                }

                for(int i = X - 1, j = Y + 1; i >= 0 && j < 8; i--, j++) //Вперед вправо
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X - 1, j = Y - 1; i >= 0 && j >= 0; i--, j--) //Вперед влево
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X + 1, j = Y + 1; i < 8 && j < 8; i++, j++) //Вниз вправо
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                for(int i = X + 1, j = Y - 1; i < 8 && j >= 0; i++, j--) //Вниз влево
                {
                    chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null) ChessMove[i, j] = 1;
                    else
                    {
                        if(White != chess[0].White) ChessMove[i, j] = 2;
                        break;
                    }
                }

                break;
            }
            case 5: //Король
            {
                //Низ
                int i, j;
                i = X + 1;
                j = Y - 1;
                if(X != 7)
                {
                    for(var k = 0; k < 3; k++)
                    {
                        if(j >= 0 && j < 8)
                        {
                            chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                            if(chess[0] == null) ChessMove[i, j] = 1;
                            else if(White != chess[0].White) ChessMove[i, j] = 2;
                        }

                        j++;
                    }
                }

                //Верх
                i = X - 1;
                j = Y - 1;
                if(X != 0)
                {
                    for(var k = 0; k < 3; k++)
                    {
                        if(j >= 0 && j < 8)
                        {
                            chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
                            if(chess[0] == null) ChessMove[i, j] = 1;
                            else if(White != chess[0].White) ChessMove[i, j] = 2;
                        }

                        j++;
                    }
                }

                //Лево
                if(Y != 0)
                {
                    chess[0] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null)
                    {
                        ChessMove[X, Y - 1] = 1;
                        if((X == 0 || X == 7) && Y == 4 && Walked == 0)
                        {
                            var c = OnBoard.Instance.Board[X, Y - 2].GetComponentInChildren<ChessMen>();
                            var c1 = OnBoard.Instance.Board[X, Y - 3].GetComponentInChildren<ChessMen>();
                            chess[0] = OnBoard.Instance.Board[X, Y - 4].GetComponentInChildren<ChessMen>();
                            if(c == null
                            && c1 == null
                            && chess[0] != null
                            && White == chess[0].White
                            && chess[0].Walked == 0
                            && chess[0].Type == 1)
                            {
                                ChessMove[X, Y - 4] = 1;
                                IsACastling = true;
                            }
                        }
                    }
                    else if(White != chess[0].White) ChessMove[X, Y - 1] = 2;
                }

                //Право
                if(Y != 7)
                {
                    chess[0] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
                    if(chess[0] == null)
                    {
                        ChessMove[X, Y + 1] = 1;
                        if((X == 0 || X == 7) && Y == 4 && Walked == 0)
                        {
                            var c = OnBoard.Instance.Board[X, Y + 2].GetComponentInChildren<ChessMen>();
                            chess[0] = OnBoard.Instance.Board[X, Y + 3].GetComponentInChildren<ChessMen>();
                            if(c == null
                            && chess[0] != null
                            && White == chess[0].White
                            && chess[0].Walked == 0
                            && chess[0].Type == 1)
                            {
                                ChessMove[X, Y + 3] = 1;
                                IsACastling = true;
                            }
                        }
                    }
                    else if(White != chess[0].White) ChessMove[X, Y + 1] = 2;
                }

                break;
            }
        }

        return ChessMove;
    }

    private void KnightChessMove(int x, int y, ref byte[,] chessMove)
    {
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            var chess = OnBoard.Instance.Board[x, y].GetComponentInChildren<ChessMen>();
            if(chess == null) chessMove[x, y] = 1;
            else if(White != chess.White) chessMove[x, y] = 2;
        }
    }
}