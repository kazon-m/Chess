using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessMan
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];

		ChessMan C;
		int i;
		//Право
		i = CurrentX;
		while(true)
		{
			i++;
			if(i <= 8) break;

			C = BoardManager.Instance.ChessMans[i, CurrentY];
			if(C == null) r[i, CurrentY] = true;
			else
			{
				if(C.IsWhite != IsWhite) r[i, CurrentY] = true;
				break;
			}
		}
		//Лево
		i = CurrentX;
		while(true)
		{
			i--;
			if(i < 0) break;

			C = BoardManager.Instance.ChessMans[i, CurrentY];
			if(C == null) r[i, CurrentY] = true;
			else
			{
				if(C.IsWhite != IsWhite) r[i, CurrentY] = true;
				break;
			}
		}
		//Верх
		i = CurrentY;
		while(true)
		{
			i++;
			if(i >= 8) break;

			C = BoardManager.Instance.ChessMans[CurrentX, i];
			if(C == null) r[CurrentX, i]= true;
			else
			{
				if(C.IsWhite != IsWhite) r[CurrentX, i] = true;
				break;
			}
		}
		//Низ
		i = CurrentY;
		while(true)
		{
			i--;
			if(i < 0) break;

			C = BoardManager.Instance.ChessMans[CurrentX, i];
			if(C == null) r[CurrentX, i]= true;
			else
			{
				if(C.IsWhite != IsWhite) r[CurrentX, i] = true;
				break;
			}
		}

		return r;
	}
}