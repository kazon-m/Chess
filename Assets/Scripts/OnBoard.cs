using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBoard : MonoBehaviour
{
    //public GameObject[,] BoardNext = new GameObject[8, 8];
    //public GameObject[,] BoardPrevious = new GameObject[8, 8];

    public bool WhiteTurn = true;

    public int Step;

    [HideInInspector]
    public GameObject ActiveChessMen;

    public List<ChessMen> AllChess = new List<ChessMen>();

    public GameObject[,] Board = new GameObject[8, 8];
    private Text MathText;

    private bool Sah;
    private Text StepText;

    private int Sx;
    private int Sy;

    private Text Turn;
    public static OnBoard Instance { set; get; }

    private void Awake()
    {
        Instance = this;
        Turn = transform.parent.GetChild(1).GetComponent<Text>();
        StepText = transform.parent.GetChild(2).GetComponent<Text>();
        MathText = transform.parent.GetChild(3).GetComponent<Text>();
        for(int i = 0, count = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
            {
                Board[i, j] = transform.GetChild(count++).gameObject;
                Board[i, j].GetComponent<OnClickBoard>().X = i;
                Board[i, j].GetComponent<OnClickBoard>().Y = j;
                var chess = Board[i, j].GetComponentInChildren<ChessMen>();
                if(chess)
                {
                    chess.X = i;
                    chess.Y = j;
                    AllChess.Add(chess);
                }
            }
        }
    }

    public void MoveChessMen(int x, int y)
    {
        var c = ActiveChessMen.GetComponentInChildren<ChessMen>();
        var ck = Board[x, y].GetComponentInChildren<ChessMen>();

        if(c.ChessMove[x, y] == 0) return;

        if(Sah) Sah = false;

        if(!ck)
        {
            if(c.ChessMove[x, y] == 2)
            {
                if(c.White) KillChess(Board[x + 1, y].GetComponentInChildren<ChessMen>());
                else KillChess(Board[x - 1, y].GetComponentInChildren<ChessMen>());
            }

            SetChessPos(c, x, y);
        }
        else
        {
            if(ck.White != WhiteTurn)
            {
                if(ck.Type == 5) return;
                KillChess(ck);
                SetChessPos(c, x, y);
            }
            else SetChessCastling(c, ck, x, y);
        }
    }

    private bool IsASah(GameObject @object)
    {
        foreach(var c in AllChess)
        {
            if(c.Type != 5) continue;
            if(@object.GetComponentInChildren<ChessMen>().PossibleMove()[c.X, c.Y] == 2) return true;
        }

        return false;
    }

    private bool IsAMat(ChessMen k)
    {
        var c = GetKing();
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
            {
                if(c.ChessMove[i, j] == 1 || c.ChessMove[i, j] == 2)
                {
                    if(!CheckKillInMove(c, i, j))
                    {
                        Debug.Log("Король может уйти от шаха " + i + j);
                        return false;
                    }
                }
            }
        }

        foreach(var cs in AllChess)
        {
            if(cs.White == WhiteTurn)
            {
                if(CheckKillInPos(cs.gameObject, k.X, k.Y))
                {
                    Debug.Log("Фигура может спасти от шаха " + cs.gameObject);
                    return false;
                }
            }
        }

        return true;
    }

    private bool CheckKillInPos(GameObject @object, int x, int y)
    {
        if(@object.GetComponentInChildren<ChessMen>().PossibleMove()[x, y] == 2) return true;
        return false;
    }

    private bool CheckKillInMove(ChessMen c, int x, int y)
    {
        c.transform.SetParent(Board[x, y].transform);
        foreach(var cs in AllChess)
        {
            if(cs.White == WhiteTurn) continue;
            if(CheckKillInPos(cs.gameObject, x, y))
            {
                c.transform.SetParent(Board[c.X, c.Y].transform);
                return true;
            }
        }

        c.transform.SetParent(Board[c.X, c.Y].transform);
        return false;
    }

    private void SelectMoves(byte[,] moves)
    {
        var acm = ActiveChessMen.GetComponentInChildren<ChessMen>();
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
            {
                if(acm.Type == 5 && CheckKillInMove(acm, i, j))
                {
                    acm.ChessMove[i, j] = 0;
                    continue;
                }

                if(acm.Type != 5 && Sah && (i != Sx || j != Sy))
                {
                    acm.ChessMove[i, j] = 0;
                    continue;
                }

                switch(moves[i, j])
                {
                    case 1:
                    {
                        SetColor(Board[i, j].gameObject, Color.green);
                        break;
                    }
                    case 2:
                    {
                        SetColor(Board[i, j].gameObject, Color.red);
                        break;
                    }
                }
            }
        }
    }

    public void SelectChessMen(GameObject @object)
    {
        if(ActiveChessMen != null) SetBaseCellColor();
        ActiveChessMen = @object;
        SetColor(@object, Color.yellow);
        SelectMoves(@object.GetComponentInChildren<ChessMen>().PossibleMove());
    }

    private void SetBaseCellColor()
    {
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
                Board[i, j].GetComponent<Image>().color = Board[i, j].GetComponent<OnClickBoard>().ColorCell;
        }
    }

    private ChessMen GetKing()
    {
        foreach(var cs in AllChess)
            if(cs.White == WhiteTurn && cs.Type == 5)
                return cs;
        return null;
    }

    private void SetColor(GameObject @object, Color objectColor) =>
        @object.GetComponent<Image>().color = objectColor;

    private void SetChessPos(ChessMen @object, int x, int y, bool cs = true)
    {
        @object.Walked++;
        Sx = @object.X = x;
        Sy = @object.Y = y;
        @object.LastStep = Step;
        @object.transform.SetParent(Board[x, y].transform);
        @object.transform.position = Board[x, y].transform.position;
        if(cs)
        {
            NextMove();
            CheckSahAndMat(@object);
            BotMove();
        }
    }

    private void CheckSahAndMat(ChessMen c)
    {
        if(IsASah(c.gameObject))
        {
            Sah = true;
            if(IsAMat(c))
            {
                Turn.text = WhiteTurn ? "Мат белым" : "Мат черным";
                return;
            }

            Turn.text = WhiteTurn ? "Шах белым" : "Шах черным";
        }
    }

    private void NextMove()
    {
        Step++;
        WhiteTurn = !WhiteTurn;
        SetColor(ActiveChessMen, ActiveChessMen.GetComponent<OnClickBoard>().ColorCell);
        Turn.text = WhiteTurn ? "Ход белых" : "Ход черных";
        StepText.text = Step + " Ход";
        ActiveChessMen = null;
        SetBaseCellColor();
    }

    public void SelectBotChessMen(GameObject @object)
    {
        ActiveChessMen = @object;
        SelectBotMoves(@object.GetComponentInChildren<ChessMen>().PossibleMove());
    }

    private void SelectBotMoves(byte[,] moves)
    {
        var acm = ActiveChessMen.GetComponentInChildren<ChessMen>();
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
            {
                if(acm.Type == 5 && CheckKillInMove(acm, i, j))
                {
                    acm.ChessMove[i, j] = 0;
                    continue;
                }

                if(acm.Type != 5 && Sah && (i != Sx || j != Sy)) { acm.ChessMove[i, j] = 0; }
            }
        }
    }

    private int GetBestChess(ChessMen cb)
    {
        var count = 0;
        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
                if(cb.ChessMove[i, j] == 1)
                    count++;
        }

        return count;
    }

    private ChessMen[] GetBestBot()
    {
        var count = 0;
        var bestChessMen = new ChessMen[2];
        foreach(var blackChess in AllChess)
        {
            if(blackChess.White) continue;
            SelectBotChessMen(blackChess.transform.parent.gameObject);

            if(count < GetBestChess(blackChess))
            {
                count = GetBestChess(blackChess);
                bestChessMen[0] = blackChess;
                Debug.Log("У "
                        + bestChessMen[0]
                        + " ["
                        + bestChessMen[0].X
                        + ","
                        + bestChessMen[0].Y
                        + "] "
                        + count
                        + " Ходов");
            }
        }

        foreach(var blackChess in AllChess)
        {
            if(blackChess.White) continue;
            SelectBotChessMen(blackChess.transform.parent.gameObject);

            foreach(var whiteChess in AllChess)
            {
                if(!whiteChess.White) continue;
                if(!CheckKillInPos(blackChess.gameObject, whiteChess.X, whiteChess.Y)) continue;
                if(CheckKillInMove(blackChess, whiteChess.X, whiteChess.Y)
                && whiteChess.Type < blackChess.Type) continue;

                if(bestChessMen[1] == null) bestChessMen[1] = whiteChess;

                Debug.Log(bestChessMen[0]
                        + " ["
                        + bestChessMen[0].X
                        + ","
                        + bestChessMen[0].Y
                        + "] "
                        + bestChessMen[0].Type
                        + " Убивает "
                        + bestChessMen[1]
                        + " ["
                        + bestChessMen[1].X
                        + ","
                        + bestChessMen[1].Y
                        + "] "
                        + bestChessMen[1].Type);

                if(bestChessMen[1].Type > whiteChess.Type) continue;

                bestChessMen[0] = blackChess;
                bestChessMen[1] = whiteChess;

                Debug.Log(bestChessMen[0]
                        + " ["
                        + bestChessMen[0].X
                        + ","
                        + bestChessMen[0].Y
                        + "] "
                        + bestChessMen[0].Type
                        + " Убивает лучше "
                        + bestChessMen[1]
                        + " ["
                        + bestChessMen[1].X
                        + ","
                        + bestChessMen[1].Y
                        + "] "
                        + bestChessMen[1].Type);
            }
        }

        return bestChessMen;
    }
    /*private void Start()
    {
        Test();
    }
    int[,] ChessMatrix = new int[, ]
    {
        {2, 3, 4, 5, 6, 4, 3, 2},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {2, 3, 4, 5, 6, 4, 3, 2},
    };
    private int Test2(ChessMen BlackChess, int Depth)
    {
        if(Depth == 0) return 0;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(BlackChess.ChessMove[i, j] == 1)
                {
                    ChessMatrix[i, j] = ChessMatrix[i, j] + ChessMatrix[BlackChess.X, BlackChess.Y];
                    ChessMatrix[BlackChess.X, BlackChess.Y] = 0;
                    //ChessMatrix[BlackChess.X, BlackChess.Y] = 0;
                }
                else if(BlackChess.ChessMove[i, j] == 2)
                    {
                        ChessMatrix[i, j] = 8;
                    }
            }
        }
        return Test2(BlackChess, Depth - 1);
    }
    private void Test()
    {

        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                MathText.text = MathText.text + " " + ChessMatrix[i,j];
            }
        }

        foreach(ChessMen BlackChess in AllChess) //Перебираем все фигуры
        {
            if(BlackChess.White) continue; // Пропускаем белые фигуры
            BlackChess.ChessMove = BlackChess.PossibleMove(); //Получаем возможные ходы фигуры
            Test2(BlackChess, 2);
        }
        MathText.text = null;
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                MathText.text = MathText.text + " " + ChessMatrix[i,j];
            }
        }

    }*/
    /*private int AlphaBeta(int color, int Depth, int alpha, int beta) 
    {
        foreach(ChessMen BlackChess in AllChess) //Перебираем все фигуры
        {
            if(BlackChess.White) continue; // Пропускаем белые фигуры
            //SelectBotChessMen(BlackChess.transform.parent.gameObject);
            BlackChess.ChessMove = BlackChess.PossibleMove(); //Получаем возможные ходы фигуры
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(BlackChess.ChessMove[i, j] == 1)
                    {
                    }
                    else if(BlackChess.ChessMove[i, j] == 2)
                    {
                        if(CheckKillInMove(BlackChess, i, j)) continue;

                    }
                }
            }
        }
    }*/
    /*private int AlphaBeta(int color, int Depth, int alpha, int beta) 
    {
            if(Depth == 0) return Evaluate(color); 
            int bestmove;
            Vector moves = GenerateMoves();
            for(int i = 0; i < moves.size(); i++)
            {
                makeMove(moves.get(i));
                eval = -AlphaBeta(-color, Depth-1, -beta, -alpha);
                unmakeMove(moves.get(i));

                if(eval >= beta) return beta;

                if(eval > alpha) 
                {
                    alpha = eval;
                    if (Depth == defaultDepth) 
                    {
                        bestmove = moves.get(i);  
                    }
                }
            }
            return alpha;
        }*/

    private void BotMove()
    {
        if(WhiteTurn) return;

        var bestChessMen = GetBestBot();

        if(bestChessMen[0] == null) return;

        SelectBotChessMen(bestChessMen[0].transform.parent.gameObject);

        if(bestChessMen[1] != null)
        {
            Debug.Log(bestChessMen[0]
                    + " ["
                    + bestChessMen[0].X
                    + ","
                    + bestChessMen[0].Y
                    + "] Убивает"
                    + " ["
                    + bestChessMen[1].X
                    + ","
                    + bestChessMen[1].Y
                    + "] ");
            MoveChessMen(bestChessMen[1].X, bestChessMen[1].Y);
        }
        else
        {
            var find = false;
            var bestMove = new int[2];
            for(var i = 0; i < 8; i++)
            {
                for(var j = 0; j < 8; j++)
                {
                    if(bestChessMen[0].ChessMove[i, j] != 1) continue;
                    if(CheckKillInMove(bestChessMen[0], i, j)) continue;
                    bestMove[0] = i;
                    bestMove[1] = j;
                    find = true;
                }
            }

            if(!find)
            {
                for(var i = 0; i < 8; i++)
                {
                    for(var j = 0; j < 8; j++)
                    {
                        if(bestChessMen[0].ChessMove[i, j] != 1) continue;
                        bestMove[0] = i;
                        bestMove[1] = j;
                        find = true;
                    }
                }
            }

            Debug.Log(bestChessMen[0]
                    + " ["
                    + bestChessMen[0].X
                    + ","
                    + bestChessMen[0].Y
                    + "] Ходит на"
                    + " ["
                    + bestMove[0]
                    + ","
                    + bestMove[1]
                    + "] ");
            MoveChessMen(bestMove[0], bestMove[1]);
        }
    }

    private void SetChessCastling(ChessMen c, ChessMen ck, int x, int y)
    {
        if(y == 7 && c.Type == 5 && ck.Type == 1)
        {
            SetChessPos(c, x, 6, false);
            SetChessPos(ck, x, 5, false);
        }
        else if(y == 0 && c.Type == 5 && ck.Type == 1)
        {
            SetChessPos(c, x, 2, false);
            SetChessPos(ck, x, 3, false);
        }
        else if(y == 4 && c.Type == 1 && ck.Type == 5)
        {
            if(c.Y == 0)
            {
                SetChessPos(c, x, 3, false);
                SetChessPos(ck, x, 2, false);
            }
            else if(c.Y == 7)
            {
                SetChessPos(c, x, 5, false);
                SetChessPos(ck, x, 6, false);
            }
        }

        NextMove();
    }

    private void KillChess(ChessMen c)
    {
        AllChess.Remove(c);
        DestroyImmediate(c.gameObject);
    }
}