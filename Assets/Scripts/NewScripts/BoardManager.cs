using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float TileSize = 1.0f;   //Для выставления фигуры по центру клетки
    private const float TileOffset = 0.5f; //Для выставления фигуры по центру клетки

    public List<GameObject> ChessManPrefabs; //Префабы фигур

    public bool IsWhiteTurn = true; //Ход белых или черных
    private List<GameObject> ActiveChessMan;
    private ChessMan SelectedChessMan;

    private int SelectionX = -1; // Координата выбранной фигуры Х
    private int SelectionY = -1; // Координата выбранной фигуры Y
    public static BoardManager Instance { set; get; }
    private bool[,] AllowedMoves { set; get; }

    public ChessMan[,] ChessMans { set; get; }

    private void Start()
    {
        Instance = this;
        SpawnAllChessMan(); //Спавним все фигуры
    }

    private void Update()
    {
        DrawChessBoard();               //Отображение доски
        if(Input.GetMouseButtonDown(0)) //Нажатие ЛКМ
        {
            UpdateSelection(); //Выделяем клетку, куда мы нажали
            if(SelectionX >= 0 && SelectionY >= 0)
            {
                if(SelectedChessMan == null)
                    SelectChessMan(SelectionX, SelectionY); //Если фигура не выбрана, то выбираем
                else MoveChessMan(SelectionX, SelectionY);  //Если выбрана, то перемещаем
            }
        }
    }

    private void SelectChessMan(int x, int y)
    {
        if(ChessMans[x, y] == null) return;                //Если клетка пустая
        if(ChessMans[x, y].IsWhite != IsWhiteTurn) return; //Выбор белой фигуры или черной

        var hasAtleastOneMove = false;

        AllowedMoves = ChessMans[x, y].PossibleMove();

        for(var i = 0; i < 8; i++)
        {
            for(var j = 0; j < 8; j++)
                if(AllowedMoves[i, j])
                    hasAtleastOneMove = true;
        }

        if(!hasAtleastOneMove) return;
        SelectedChessMan = ChessMans[x, y]; //Выбираем клетку с фигурой
        BoardHighLights.Instance.HighLightAllowedMoves(AllowedMoves);
    }

    private void MoveChessMan(int x, int y)
    {
        if(AllowedMoves[x, y])
        {
            var c = ChessMans[x, y];
            if(c != null && c.IsWhite != IsWhiteTurn)
            {
                if(c.GetType() == typeof(King))
                {
                    EndGame();
                    return;
                }

                ActiveChessMan.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            ChessMans[SelectedChessMan.CurrentX, SelectedChessMan.CurrentY] =
                null;                                                  //Обнуляем значение предыдущей клетки
            SelectedChessMan.transform.position = GetTileCenter(x, y); //Перемещаем фигуру на новую клетку
            SelectedChessMan.SetPosition(x, y);
            ChessMans[x, y] = SelectedChessMan; //Присваиваем новой клетке фигуру
            IsWhiteTurn = !IsWhiteTurn;         //Передаем ход противоположному игроку
        }

        BoardHighLights.Instance.HideHighLights();
        SelectedChessMan = null; //Обнуляем выделение фигуры
    }

    private void UpdateSelection()
    {
        if(!Camera.main) return;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f,
                           LayerMask.GetMask("ChessPlane")))
        {
            SelectionX = (int) hit.point.x;
            SelectionY = (int) hit.point.z;
        }
        else SelectionX = SelectionY = -1;
    }

    private void SpawnChessMan(int index, int x, int y)
    {
        var position = GetTileCenter(x, y);
        var orientation = Quaternion.Euler(0, 0, 0);
        if(index == 0 || index == 6) position.y += 0.33f;
        else if(index == 3 || index == 9)
        {
            position.y += 0.55f;
            orientation = Quaternion.Euler(90, 0, 0);
        }
        else if(index == 4) orientation = Quaternion.Euler(0, 90, 0);
        else if(index == 5 || index == 11) orientation = Quaternion.Euler(90, 0, 0);
        else if(index == 10) orientation = Quaternion.Euler(0, -90, 0);

        var chessGO = Instantiate(ChessManPrefabs[index], position, orientation);
        chessGO.transform.SetParent(transform);

        ChessMans[x, y] = chessGO.GetComponent<ChessMan>();
        ChessMans[x, y].SetPosition(x, y);
        ActiveChessMan.Add(chessGO);
    }

    private void SpawnAllChessMan()
    {
        ActiveChessMan = new List<GameObject>();
        ChessMans = new ChessMan[8, 8];

        SpawnChessMan(0, 3, 0);
        SpawnChessMan(1, 4, 0);
        SpawnChessMan(2, 0, 0);
        SpawnChessMan(2, 7, 0);
        SpawnChessMan(3, 2, 0);
        SpawnChessMan(3, 5, 0);
        SpawnChessMan(4, 1, 0);
        SpawnChessMan(4, 6, 0);

        SpawnChessMan(6, 4, 7);
        SpawnChessMan(7, 3, 7);
        SpawnChessMan(8, 0, 7);
        SpawnChessMan(8, 7, 7);
        SpawnChessMan(9, 2, 7);
        SpawnChessMan(9, 5, 7);
        SpawnChessMan(10, 1, 7);
        SpawnChessMan(10, 6, 7);

        for(var i = 0; i < 8; i++)
        {
            SpawnChessMan(5, i, 1);
            SpawnChessMan(11, i, 6);
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        var origin = Vector3.zero;
        origin.x += TileSize * x + TileOffset;
        origin.z += TileSize * y + TileOffset;
        return origin;
    }

    private void DrawChessBoard()
    {
        var widthLine = Vector3.right * 8;
        var heightLine = Vector3.forward * 8;

        for(var i = 0; i <= 8; i++)
        {
            var start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            start = Vector3.right * i;
            Debug.DrawLine(start, start + heightLine);
        }

        if(SelectionX >= 0 && SelectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * SelectionY + Vector3.right * SelectionX,
                           Vector3.forward * (SelectionY + 1) + Vector3.right * (SelectionX + 1));
            Debug.DrawLine(Vector3.forward * (SelectionY + 1) + Vector3.right * SelectionX,
                           Vector3.forward * SelectionY + Vector3.right * (SelectionX + 1));
        }
    }

    private void EndGame()
    {
        if(IsWhiteTurn) Debug.Log("White Team Wins");
        else Debug.Log("Black Team Wins");

        foreach(var go in ActiveChessMan) Destroy(go);
        IsWhiteTurn = true;
        BoardHighLights.Instance.HideHighLights();
        SpawnAllChessMan();
    }
}