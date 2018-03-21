using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessMen : MonoBehaviour 
{
	public static ChessMen Instance { get; set; }

	public int Type; //Тип фигуры Пешка, Король и тд.
	public bool White; //Цвет фигуры белый-черный
	public int LastStep; //Последний ход фигуры
	public int Walked = 0; //Сколько ходов ходила фигура
	public bool IsACastling; //Есть ли возможность рокировки

	public int X;
	public int Y;

	public int MX;
	public int MY;

	public byte[,] ChessMove = new byte[8, 8];
	//public byte[,] ChessMovePrevious = new byte[8, 8];
	//public byte[,] ChessMoveNext = new byte[8, 8];
	// 0 - Хода нет
	// 1 - Есть ход
	// 2 - Может убить

	private void Awake()
	{
		Instance = this;
	}
		
	public byte[,] PossibleMove()
	{
		IsACastling = false;
		ChessMove = new byte[8, 8];
		ChessMen [] Chess = new ChessMen[2];
		switch(Type)
		{
			case 0: // Пешка
			{
				if(White)
				{
					if(X != 0 && Y != 0) // Вперед влево
					{
						Chess[0] = OnBoard.Instance.Board[X - 1, Y - 1].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
						if(Chess[0] != null && White != Chess[0].White) ChessMove[X - 1, Y - 1] = 2;
						else if(Chess[1] != null && White != Chess[1].White && Chess[1].Walked == 1 && OnBoard.Instance.Step == Chess[1].LastStep+1) ChessMove[X - 1, Y - 1] = 2;
					}
					if(X != 0 && Y != 7) // Вперед вправо
					{
						Chess[0] = OnBoard.Instance.Board[X - 1, Y + 1].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
						if(Chess[0] != null && White != Chess[0].White) ChessMove[X - 1, Y + 1] = 2;
						else if(Chess[1] != null && White != Chess[1].White && Chess[1].Walked == 1 && OnBoard.Instance.Step == Chess[1].LastStep+1) ChessMove[X - 1, Y + 1] = 2;
					}
					if(X != 0 && X != 6) //Вперед
					{
						Chess[0] = OnBoard.Instance.Board[X - 1, Y].GetComponentInChildren<ChessMen>();
						if(Chess[0] == null) ChessMove[X - 1, Y] = 1;
					}
					else if(X == 6) //Первый ход, два хода вперед
					{
						Chess[0] = OnBoard.Instance.Board[X - 1, Y].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X - 2, Y].GetComponentInChildren<ChessMen>();
						if(Chess[0] == null) ChessMove[X - 1, Y] = 1;
						if(Chess[0] == null && Chess[1] == null) ChessMove[X - 2, Y] = 1;
					}
				}
				else
				{
					if(X != 7 && Y != 0) // Вперед влево
					{
						Chess[0] = OnBoard.Instance.Board[X + 1, Y - 1].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
						if(Chess[0] != null && White != Chess[0].White) ChessMove[X + 1, Y - 1] = 2;
						else if(Chess[1] != null && White != Chess[1].White && Chess[1].Walked == 1 && OnBoard.Instance.Step == Chess[1].LastStep+1) ChessMove[X + 1, Y - 1] = 2;
					}
					if(X != 7 && Y != 7) // Вперед вправо
					{
						Chess[0] = OnBoard.Instance.Board[X + 1, Y + 1].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
						if(Chess[0] != null && White != Chess[0].White) ChessMove[X + 1, Y + 1] = 2;
						else if(Chess[1] != null && White != Chess[1].White && Chess[1].Walked == 1 && OnBoard.Instance.Step == Chess[1].LastStep+1) ChessMove[X + 1, Y + 1] = 2;
					}
					if(X != 7 && X != 1) //Вперед
					{
						Chess[0] = OnBoard.Instance.Board[X + 1, Y].GetComponentInChildren<ChessMen>();
						if(Chess[0] == null) ChessMove[X + 1, Y] = 1;
					}
					else if(X == 1) //Первый ход, два хода вперед
					{
						Chess[0] = OnBoard.Instance.Board[X + 1, Y].GetComponentInChildren<ChessMen>();
						Chess[1] = OnBoard.Instance.Board[X + 2, Y].GetComponentInChildren<ChessMen>();
						if(Chess[0] == null) ChessMove[X + 1, Y] = 1;
						if(Chess[0] == null && Chess[1] == null) ChessMove[X + 2, Y] = 1;
					}
				}
				break;
			}
			case 1: //Ладья
			{
				for(int i = Y+1; i < 8; i++) //Право
				{
					Chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[X, i] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[X, i] = 2;
						else
						{
							if(Chess[0].Walked == 0 && Chess[0].Type == 5)
							{
								ChessMove[X, i] = 1;
								IsACastling = true;
							}
						}
						break;
					}
				}
				for(int i = Y-1; i >= 0; i--) //Лево
				{
					Chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[X, i] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[X, i] = 2;
						else
						{
							if(Chess[0].Walked == 0 && Chess[0].Type == 5)
							{
								ChessMove[X, i] = 1;
								IsACastling = true;
							}
						}
						break;
					}
				}
				for(int i = X-1; i >= 0; i--) //Верх
				{
					Chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, Y] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[i, Y] = 2;
						break;
					}
				}
				for(int i = X+1; i < 8; i++) //Низ
				{
					Chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, Y] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[i, Y] = 2;
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
				for(int i = X-1, j = Y+1; i >= 0 && j < 8; i--, j++) //Вперед вправо
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X-1, j = Y-1; i >= 0 && j >= 0; i--, j--) //Вперед влево
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X+1, j = Y+1; i < 8 && j < 8; i++, j++) //Вниз вправо
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X+1, j = Y-1; i < 8 && j >= 0; i++, j--) //Вниз влево
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				break;
			}
			case 4: //Королева
			{
				for(int i = Y+1; i < 8; i++) //Право
				{
					Chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[X, i] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[X, i] = 2;
						break;
					}
				}
				for(int i = Y-1; i >= 0; i--) //Лево
				{
					Chess[0] = OnBoard.Instance.Board[X, i].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[X, i] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[X, i] = 2;
						break;
					}
				}
				for(int i = X-1; i >= 0; i--) //Верх
				{
					Chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, Y] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[i, Y] = 2;
						break;
					}
				}
				for(int i = X+1; i < 8; i++) //Низ
				{
					Chess[0] = OnBoard.Instance.Board[i, Y].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, Y] = 1;
					else
					{
						if(Chess[0].White != White) ChessMove[i, Y] = 2;
						break;
					}
				}

				for(int i = X-1, j = Y+1; i >= 0 && j < 8; i--, j++) //Вперед вправо
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X-1, j = Y-1; i >= 0 && j >= 0; i--, j--) //Вперед влево
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X+1, j = Y+1; i < 8 && j < 8; i++, j++) //Вниз вправо
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
						break;
					}
				}
				for(int i = X+1, j = Y-1; i < 8 && j >= 0; i++, j--) //Вниз влево
				{
					Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null) ChessMove[i, j] = 1;
					else
					{
						if(White != Chess[0].White) ChessMove[i, j] = 2;
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
					for(int k = 0; k < 3; k++)
					{
						if(j >= 0 && j < 8)
						{
							Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
							if(Chess[0] == null) ChessMove[i, j] = 1;
							else if(White != Chess[0].White) ChessMove[i, j] = 2;
						}
						j++;
					}
				}
				//Верх
				i = X - 1;
				j = Y - 1;
				if(X != 0)
				{
					for(int k = 0; k < 3; k++)
					{
						if(j >= 0 && j < 8)
						{
							Chess[0] = OnBoard.Instance.Board[i, j].GetComponentInChildren<ChessMen>();
							if(Chess[0] == null) ChessMove[i, j] = 1;
							else if(White != Chess[0].White) ChessMove[i, j] = 2;
						}
						j++;
					}
				}
				//Лево
				if(Y != 0)
				{
					Chess[0] = OnBoard.Instance.Board[X, Y - 1].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null)
					{
						ChessMove[X, Y - 1] = 1;
						if((X == 0 || X == 7) && Y == 4 && Walked == 0)
						{
							ChessMen C = OnBoard.Instance.Board[X, Y - 2].GetComponentInChildren<ChessMen>();
							ChessMen C1 = OnBoard.Instance.Board[X, Y - 3].GetComponentInChildren<ChessMen>();
							Chess[0] = OnBoard.Instance.Board[X, Y - 4].GetComponentInChildren<ChessMen>();
							if(C == null && C1 == null && Chess[0] != null && White == Chess[0].White && Chess[0].Walked == 0 && Chess[0].Type == 1)
							{
								ChessMove[X, Y - 4] = 1;
								IsACastling = true;
							}
						}
					}
					else if(White != Chess[0].White) ChessMove[X, Y - 1] = 2;
				}
				//Право
				if(Y != 7)
				{
					Chess[0] = OnBoard.Instance.Board[X, Y + 1].GetComponentInChildren<ChessMen>();
					if(Chess[0] == null)
					{
						ChessMove[X, Y + 1] = 1;
						if((X == 0 || X == 7) && Y == 4 && Walked == 0)
						{
							ChessMen C = OnBoard.Instance.Board[X, Y + 2].GetComponentInChildren<ChessMen>();
							Chess[0] = OnBoard.Instance.Board[X, Y + 3].GetComponentInChildren<ChessMen>();
							if(C == null && Chess[0] != null && White == Chess[0].White && Chess[0].Walked == 0 && Chess[0].Type == 1)
							{
								ChessMove[X, Y + 3] = 1;
								IsACastling = true;
							}
						}
					}
					else if(White != Chess[0].White) ChessMove[X, Y + 1] = 2;
				}
				break;
			}
		}
		return ChessMove;
	}

	private void KnightChessMove(int X, int Y, ref byte[,] ChessMove)
	{
		if(X >= 0 && X < 8 && Y >= 0 && Y < 8)
		{
			ChessMen Chess = OnBoard.Instance.Board[X, Y].GetComponentInChildren<ChessMen>();
			if(Chess == null) ChessMove[X, Y] = 1;
			else if(White != Chess.White) ChessMove[X, Y] = 2;
		}
	}
}