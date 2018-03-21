using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pawn : ChessMan 
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];
		ChessMan C, C2;

		if(IsWhite)
		{
			if(CurrentX != 0 && CurrentY != 7)
			{
				C = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY + 1];
				if(C != null && !C.IsWhite)
				{
					r[CurrentX - 1, CurrentY + 1] = true;
				}
			}
			if(CurrentX != 7 && CurrentY != 7)
			{
				C = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY + 1];
				if(C != null && !C.IsWhite)
				{
					r[CurrentX + 1, CurrentY + 1] = true;
				}
			}
			if(CurrentY != 7)
			{
				C = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 1];
				if(C == null) r[CurrentX, CurrentY + 1] = true;
			}
			if(CurrentY == 1)
			{
				C = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 1];
				C2 = BoardManager.Instance.ChessMans[CurrentX, CurrentY + 2];
				if(C == null && C2 == null) r[CurrentX, CurrentY + 2] = true;
			}
		}
		else
		{
			if(CurrentX != 0 && CurrentY != 0)
			{
				C = BoardManager.Instance.ChessMans[CurrentX - 1, CurrentY - 1];
				if(C != null && C.IsWhite)
				{
					r[CurrentX - 1, CurrentY - 1] = true;
				}
			}
			if(CurrentX != 7 && CurrentY != 0)
			{
				C = BoardManager.Instance.ChessMans[CurrentX + 1, CurrentY - 1];
				if(C != null && C.IsWhite)
				{
					r[CurrentX + 1, CurrentY - 1] = true;
				}
			}
			if(CurrentY != 0)
			{
				C = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 1];
				if(C == null) r[CurrentX, CurrentY - 1] = true;
			}
			if(CurrentY == 6)
			{
				C = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 1];
				C2 = BoardManager.Instance.ChessMans[CurrentX, CurrentY - 2];
				if(C == null && C2 == null) r[CurrentX, CurrentY - 2] = true;
			}
		}

		return r;
	}
}