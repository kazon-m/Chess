using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessMan 
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];

		ChessMan C;
		int i, j;
		//Верх
		i = CurrentX - 1;
		j = CurrentY + 1;
		if(CurrentY != 7)
		{
			for(int k = 0; k < 3; k++)
			{
				if(i >= 0 || i < 8)
				{
					C = BoardManager.Instance.ChessMans[i, j];
					if(C == null) r[i, j] = true;
					else if(IsWhite != C.IsWhite) r[i, j] = true;
				}
				i++;
			}
		}
		//Низ
		i = CurrentX - 1;
		j = CurrentY - 1;
		if(CurrentY != 0)
		{
			for(int k = 0; k < 3; k++)
			{
				if(i >= 0 || i < 8)
				{
					C = BoardManager.Instance.ChessMans[i, j];
					if(C == null) r[i, j] = true;
					else if(IsWhite != C.IsWhite) r[i, j] = true;
				}
				i++;
			}
		}
		//Лево
		if(CurrentX != 0)
		{
			C = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY];
			if(C == null) r[CurrentX - 1, CurrentY] = true;
			else if(IsWhite != C.IsWhite) r[CurrentX - 1, CurrentY] = true;
		}
		//Право
		if(CurrentX != 7)
		{
			C = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY];
			if(C == null) r[CurrentX + 1, CurrentY] = true;
			else if(IsWhite != C.IsWhite) r[CurrentX + 1, CurrentY] = true;
		}
		return r;
	}
}