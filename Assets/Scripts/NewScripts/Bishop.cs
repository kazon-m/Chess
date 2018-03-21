using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessMan
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];

		ChessMan C;
		int i, j;

		//Верх лево
		i = CurrentX;
		j = CurrentY;
		while(true)
		{
			i--;
			j++;
			if(i < 0 || j >= 8) break;
			C = BoardManager.Instance.ChessMans[i, j];
			if(C == null) r[i, j] = true;
			else
			{
				if(IsWhite != C.IsWhite) r[i, j] = true;
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
			C = BoardManager.Instance.ChessMans[i, j];
			if(C == null) r[i, j] = true;
			else
			{
				if(IsWhite != C.IsWhite) r[i, j] = true;
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
			C = BoardManager.Instance.ChessMans[i, j];
			if(C == null) r[i, j] = true;
			else
			{
				if(IsWhite != C.IsWhite) r[i, j] = true;
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
			if(i >= 8|| j < 0) break;
			C = BoardManager.Instance.ChessMans[i, j];
			if(C == null) r[i, j] = true;
			else
			{
				if(IsWhite != C.IsWhite) r[i, j] = true;
				break;
			}
		}

		return r;
	}
}