using UnityEngine;
using UnityEngine.UI;

public class OnClickBoard : MonoBehaviour
{
    public int X;
    public int Y;

    public Color ColorCell;
    public static OnClickBoard Instance { set; get; }

    private void Awake()
    {
        Instance = this;
        ColorCell = GetComponent<Image>().color;
    }

    private void OnClick()
    {
        var chess = GetComponentInChildren<ChessMen>();

        if(chess) //Выбрали клетку на которой шахмата
        {
            if(!OnBoard.Instance.ActiveChessMen)
            {
                if(!chess.White /*!= OnBoard.Instance.WhiteTurn*/) return;
                OnBoard.Instance.SelectChessMen(gameObject);
            }
            else
            {
                var acm = OnBoard.Instance.ActiveChessMen.GetComponentInChildren<ChessMen>();
                if(chess.White != OnBoard.Instance.WhiteTurn) OnBoard.Instance.MoveChessMen(X, Y);
                else
                {
                    if(acm.Type == 5 && chess.Type == 1 && acm.IsACastling
                    || acm.Type == 1 && chess.Type == 5 && acm.IsACastling)
                        OnBoard.Instance.MoveChessMen(X, Y);
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