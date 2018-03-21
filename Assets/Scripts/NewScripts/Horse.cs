using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : ChessMan
{
	public override bool[,] PossibleMove()
	{
		bool[,] r = new bool[8, 8];

		//Лево
		KnightMove(CurrentX - 1, CurrentY + 2, ref r);

		//Право
		KnightMove(CurrentX + 1, CurrentY + 2, ref r);

		//Право верх
		KnightMove(CurrentX + 2, CurrentY + 1, ref r);

		//Право низ
		KnightMove(CurrentX + 2, CurrentY - 1, ref r);

		//Лево низ
		KnightMove(CurrentX - 1, CurrentY - 2, ref r);

		//низ Право
		KnightMove(CurrentX + 1, CurrentY - 2, ref r);

		//Лево верх
		KnightMove(CurrentX - 2, CurrentY + 1, ref r);

		//Лево низ
		KnightMove(CurrentX - 2, CurrentY - 1, ref r);
		return r;
	}

	public void KnightMove(int X, int Y, ref bool[,] r)
	{
		ChessMan C;
		if(X >= 0 && X < 8 && Y >= 0 && Y < 8)
		{
			C = BoardManager.Instance.ChessMans[X, Y];
			if(C == null) r[X, Y] = true;
			else if(IsWhite != C.IsWhite) r[X, Y] = true;
		}
	}
}