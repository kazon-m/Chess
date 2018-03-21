using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessMan : MonoBehaviour 
{
	public int CurrentX { set; get; }
	public int CurrentY { set; get; }
	public bool IsWhite;

	public void SetPosition(int X, int Y)
	{
		CurrentX = X;
		CurrentY = Y;
	}
	public virtual bool[,] PossibleMove()
	{
		return new bool[8, 8];
	}
}