using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OnBoard : MonoBehaviour 
{
	public static OnBoard Instance { set; get; }

	public GameObject[,] Board = new GameObject[8, 8];
	//public GameObject[,] BoardNext = new GameObject[8, 8];
	//public GameObject[,] BoardPrevious = new GameObject[8, 8];

	public bool WhiteTurn = true;

	private Text Turn;
	private Text StepText;
	private Text MathText;

	private bool Sah;

	private int SX;
	private int SY;

	public int Step;
	[HideInInspector]
	public GameObject ActiveChessMen;

	public List<ChessMen> AllChess = new List<ChessMen>();

	private void Awake()
	{
		Instance = this;
		Turn = transform.parent.GetChild(1).GetComponent<Text>();
		StepText = transform.parent.GetChild(2).GetComponent<Text>();
		MathText = transform.parent.GetChild(3).GetComponent<Text>();
		for(int i = 0, Count = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				Board[i, j] = transform.GetChild(Count++).gameObject;
				Board[i, j].GetComponent<OnClickBoard>().X = i;
				Board[i, j].GetComponent<OnClickBoard>().Y = j;
				ChessMen Chess = Board[i, j].GetComponentInChildren<ChessMen>();
				if(Chess)
				{
					Chess.X = i;
					Chess.Y = j;
					AllChess.Add(Chess);
				}
			}
		}
	}

	public void MoveChessMen(int X, int Y)
	{
		ChessMen C = ActiveChessMen.GetComponentInChildren<ChessMen>();
		ChessMen CK = Board[X, Y].GetComponentInChildren<ChessMen>();

		if(C.ChessMove[X, Y] == 0) return;

		if(Sah) Sah = false;

		if(!CK)
		{
			if(C.ChessMove[X, Y] == 2)
			{
				if(C.White) KillChess(Board[X + 1, Y].GetComponentInChildren<ChessMen>());
				else KillChess(Board[X - 1, Y].GetComponentInChildren<ChessMen>());
			}
			SetChessPos(C, X, Y);
		}
		else
		{
			if(CK.White != WhiteTurn)
			{
				if(CK.Type == 5) return;
				KillChess(CK);
				SetChessPos(C, X, Y);
			}
			else SetChessCastling(C, CK, X, Y);
		}
	}

	private bool IsASah(GameObject Object)
	{
		foreach(ChessMen C in AllChess)
		{
			if(C.Type != 5) continue;
			if(Object.GetComponentInChildren<ChessMen>().PossibleMove()[C.X, C.Y] == 2) return true;
		}
		return false;
	}

	private bool IsAMat(ChessMen K)
	{
		ChessMen C = GetKing();
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(C.ChessMove[i, j] == 1 || C.ChessMove[i, j] == 2)
				{
					if(!CheckKillInMove(C, i, j))
					{
						Debug.Log("Король может уйти от шаха " + i + j);
						return false;
					}
				}
			}
		}
		foreach(ChessMen CS in AllChess)
		{
			if(CS.White == WhiteTurn)
			{
				if(CheckKillInPos(CS.gameObject, K.X, K.Y))
				{
					Debug.Log("Фигура может спасти от шаха " + CS.gameObject);
					return false;
				}
			}
		}
		return true;
	}

	private bool CheckKillInPos(GameObject Object, int X, int Y)
	{
		if(Object.GetComponentInChildren<ChessMen>().PossibleMove()[X, Y] == 2) return true;
		return false;
	}

	private bool CheckKillInMove(ChessMen C, int X, int Y)
	{
		C.transform.SetParent(Board[X, Y].transform);
		foreach(ChessMen CS in AllChess)
		{
			if(CS.White == WhiteTurn) continue;
			if(CheckKillInPos(CS.gameObject, X, Y))
			{
				C.transform.SetParent(Board[C.X, C.Y].transform);
				return true;
			}
		}
		C.transform.SetParent(Board[C.X, C.Y].transform);
		return false;
	}

	private void SelectMoves(byte[,] Moves)
	{
		ChessMen ACM = ActiveChessMen.GetComponentInChildren<ChessMen>();
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(ACM.Type == 5 && CheckKillInMove(ACM, i, j))
				{
					ACM.ChessMove[i, j] = 0;
					continue;
				}
				if(ACM.Type != 5 && Sah && (i != SX || j != SY))
				{
					ACM.ChessMove[i, j] = 0;
					continue;
				}
				switch(Moves[i, j])
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

	public void SelectChessMen(GameObject Object)
	{
		if(ActiveChessMen != null) SetBaseCellColor();
		ActiveChessMen = Object;
		SetColor(Object, Color.yellow);
		SelectMoves(Object.GetComponentInChildren<ChessMen>().PossibleMove());
	}

	private void SetBaseCellColor()
	{
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++) Board[i, j].GetComponent<Image>().color = Board[i, j].GetComponent<OnClickBoard>().ColorCell;
		}
	}

	private ChessMen GetKing()
	{
		foreach(ChessMen CS in AllChess)
		{
			if(CS.White == WhiteTurn && CS.Type == 5) return CS;
		}
		return null;
	}

	private void SetColor(GameObject Object, Color ObjectColor)
	{
		Object.GetComponent<Image>().color = ObjectColor;
	}
		
	private void SetChessPos(ChessMen Object, int X, int Y, bool CS = true)
	{
		Object.Walked++;
		SX = Object.X = X; 
		SY = Object.Y = Y;
		Object.LastStep = Step;
		Object.transform.SetParent(Board[X, Y].transform);
		Object.transform.position = Board[X, Y].transform.position;
		if(CS)
		{
			NextMove();
			CheckSahAndMat(Object);
			BotMove();
		}
	}

	private void CheckSahAndMat(ChessMen C)
	{
		if(IsASah(C.gameObject))
		{
			Sah = true;
			if(IsAMat(C))
			{
				Turn.text = (WhiteTurn) ? ("Мат белым") : ("Мат черным");
				return;
			}
			Turn.text = (WhiteTurn) ? ("Шах белым") : ("Шах черным");
		}
	}

	private void NextMove()
	{
		Step++;
		WhiteTurn = !WhiteTurn;
		SetColor(ActiveChessMen, ActiveChessMen.GetComponent<OnClickBoard>().ColorCell);
		Turn.text = (WhiteTurn) ? ("Ход белых") : ("Ход черных");
		StepText.text = Step + " Ход";
		ActiveChessMen = null;
		SetBaseCellColor();
	}

	public void SelectBotChessMen(GameObject Object)
	{
		ActiveChessMen = Object;
		SelectBotMoves(Object.GetComponentInChildren<ChessMen>().PossibleMove());
	}

	private void SelectBotMoves(byte[,] Moves)
	{
		ChessMen ACM = ActiveChessMen.GetComponentInChildren<ChessMen>();
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(ACM.Type == 5 && CheckKillInMove(ACM, i, j))
				{
					ACM.ChessMove[i, j] = 0;
					continue;
				}
				if(ACM.Type != 5 && Sah && (i != SX || j != SY))
				{
					ACM.ChessMove[i, j] = 0;
					continue;
				}
			}
		}
	}

	private int GetBestChess(ChessMen CB)
	{
		int Count = 0;
		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(CB.ChessMove[i, j] == 1) Count++;
			}
		}
		return Count;
	}
		
	private ChessMen[] GetBestBot()
	{
		int Count = 0;
		ChessMen [] BestChessMen = new ChessMen[2];
		foreach(ChessMen BlackChess in AllChess)
		{
			if(BlackChess.White) continue;
			SelectBotChessMen(BlackChess.transform.parent.gameObject);

			if(Count < GetBestChess(BlackChess))
			{
				Count = GetBestChess(BlackChess);
				BestChessMen[0] = BlackChess;
				Debug.Log("У " + BestChessMen[0] + " [" + BestChessMen[0].X + "," + BestChessMen[0].Y + "] " + Count + " Ходов");
			}

		}
		foreach(ChessMen BlackChess in AllChess)
		{
			if(BlackChess.White) continue;
			SelectBotChessMen(BlackChess.transform.parent.gameObject);

			foreach(ChessMen WhiteChess in AllChess)
			{
				if(!WhiteChess.White) continue;
				if(!CheckKillInPos(BlackChess.gameObject, WhiteChess.X, WhiteChess.Y)) continue;
				if(CheckKillInMove(BlackChess, WhiteChess.X, WhiteChess.Y) && WhiteChess.Type < BlackChess.Type) continue;

				if(BestChessMen[1] == null) BestChessMen[1] = WhiteChess;

				Debug.Log(BestChessMen[0] + " [" + BestChessMen[0].X + "," + BestChessMen[0].Y + "] " + BestChessMen[0].Type + " Убивает " +
				          BestChessMen[1] + " [" + BestChessMen[1].X + "," + BestChessMen[1].Y + "] " + BestChessMen[1].Type);
				
				if(BestChessMen[1].Type > WhiteChess.Type) continue;

				BestChessMen[0] = BlackChess;
				BestChessMen[1] = WhiteChess;

				Debug.Log(BestChessMen[0] + " [" + BestChessMen[0].X + "," + BestChessMen[0].Y + "] " + BestChessMen[0].Type + " Убивает лучше " +
				          BestChessMen[1] + " [" + BestChessMen[1].X + "," + BestChessMen[1].Y + "] " + BestChessMen[1].Type);
			}
		}
			
		return BestChessMen;
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

		ChessMen [] BestChessMen = GetBestBot();

		if(BestChessMen[0] == null) return;

		SelectBotChessMen(BestChessMen[0].transform.parent.gameObject);

		if(BestChessMen[1] != null) 
		{
			Debug.Log(BestChessMen[0] + " [" + BestChessMen[0].X + "," + BestChessMen[0].Y 
				+ "] Убивает" + " [" + BestChessMen[1].X + "," + BestChessMen[1].Y + "] ");
			MoveChessMen(BestChessMen[1].X, BestChessMen[1].Y);
		}
		else
		{
			bool find = false;
			int[] BestMove = new int[2];
			for(int i = 0; i < 8; i++)
			{
				for(int j = 0; j < 8; j++)
				{
					if(BestChessMen[0].ChessMove[i, j] != 1) continue;
					if(CheckKillInMove(BestChessMen[0], i, j)) continue;
					BestMove[0] = i;
					BestMove[1] = j;
					find = true;
				}
			}
			if(!find)
			{
				for(int i = 0; i < 8; i++)
				{
					for(int j = 0; j < 8; j++)
					{
						if(BestChessMen[0].ChessMove[i, j] != 1) continue;
						BestMove[0] = i;
						BestMove[1] = j;
						find = true;
					}
				}
			}
			Debug.Log(BestChessMen[0] + " [" + BestChessMen[0].X + "," + BestChessMen[0].Y 
				+ "] Ходит на" + " [" + BestMove[0] + "," + BestMove[1] + "] ");
			MoveChessMen(BestMove[0], BestMove[1]);
		}
	}

	private void SetChessCastling(ChessMen C, ChessMen CK, int X, int Y)
	{
		if(Y == 7 && C.Type == 5 && CK.Type == 1)
		{
			SetChessPos(C, X, 6, false);
			SetChessPos(CK, X, 5, false);
		}
		else if(Y == 0 && C.Type == 5 && CK.Type == 1)
		{
			SetChessPos(C, X, 2, false);
			SetChessPos(CK, X, 3, false);
		}
		else if(Y == 4 && C.Type == 1 && CK.Type == 5)
		{
			if(C.Y == 0)
			{
				SetChessPos(C, X, 3, false);
				SetChessPos(CK, X, 2, false);
			}
			else if(C.Y == 7)
			{
				SetChessPos(C, X, 5, false);
				SetChessPos(CK, X, 6, false);
			}
		}
		NextMove();
	}

	private void KillChess(ChessMen C)
	{
		AllChess.Remove(C);
		DestroyImmediate(C.gameObject);
	}
}