using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OnClickBoard : MonoBehaviour 
{
	public static OnClickBoard Instance { set; get; }

	public int X;
	public int Y;

	public Color ColorCell;

	private void Awake()
	{
		Instance = this;
		ColorCell = GetComponent<Image>().color;
	}

	private void OnClick()
	{
		ChessMen Chess = GetComponentInChildren<ChessMen>();

		if(Chess) //Выбрали клетку на которой шахмата
		{
			if(!OnBoard.Instance.ActiveChessMen)
			{
				if(!Chess.White /*!= OnBoard.Instance.WhiteTurn*/) return;
				else OnBoard.Instance.SelectChessMen(gameObject);
			}
			else
			{
				ChessMen ACM = OnBoard.Instance.ActiveChessMen.GetComponentInChildren<ChessMen>();
				if(Chess.White != OnBoard.Instance.WhiteTurn) OnBoard.Instance.MoveChessMen(X, Y);
				else
				{
					if(ACM.Type == 5 && Chess.Type == 1 && ACM.IsACastling
					|| ACM.Type == 1 && Chess.Type == 5 && ACM.IsACastling) OnBoard.Instance.MoveChessMen(X, Y);
					else OnBoard.Instance.SelectChessMen(gameObject);
				}
			}
		}
		else //Выбрали пустую клетку
		{
			if(OnBoard.Instance.ActiveChessMen != null) OnBoard.Instance.MoveChessMen(X, Y);
		}
	}
}