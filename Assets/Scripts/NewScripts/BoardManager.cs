using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour 
{
	public static BoardManager Instance { set; get; }
	private bool[,] AllowedMoves { set; get; }

	public ChessMan[,] ChessMans { set; get; }
	private ChessMan SelectedChessMan;

	private const float TILE_SIZE = 1.0f; //Для выставления фигуры по центру клетки
	private const float TILE_OFFSET = 0.5f; //Для выставления фигуры по центру клетки

	private int SelectionX = -1; // Координата выбранной фигуры Х
	private int SelectionY = -1; // Координата выбранной фигуры Y

	public List<GameObject> ChessManPrefabs; //Префабы фигур
	private List<GameObject> ActiveChessMan;

	public bool IsWhiteTurn = true; //Ход белых или черных

	private void Start()
	{
		Instance = this;
		SpawnAllChessMan(); //Спавним все фигуры
	}
	private void Update()
	{
		DrawChessBoard(); //Отображение доски
		if(Input.GetMouseButtonDown(0)) //Нажатие ЛКМ
		{
			UpdateSelection(); //Выделяем клетку, куда мы нажали
			if(SelectionX >= 0 && SelectionY >= 0)
			{
				if(SelectedChessMan == null) SelectChessMan(SelectionX, SelectionY); //Если фигура не выбрана, то выбираем
				else MoveChessMan(SelectionX, SelectionY); //Если выбрана, то перемещаем
			}
		}
	}
	private void SelectChessMan(int X, int Y)
	{
		if(ChessMans[X, Y] == null) return; //Если клетка пустая
		if(ChessMans[X, Y].IsWhite != IsWhiteTurn) return; //Выбор белой фигуры или черной

		bool HasAtleastOneMove = false;

		AllowedMoves = ChessMans[X, Y].PossibleMove();

		for(int i = 0; i < 8; i++)
		{
			for(int j = 0; j < 8; j++)
			{
				if(AllowedMoves[i, j]) HasAtleastOneMove = true;
			}
		}

		if(!HasAtleastOneMove) return;
		SelectedChessMan = ChessMans[X, Y]; //Выбираем клетку с фигурой
		BoardHighLights.Instance.HighLightAllowedMoves(AllowedMoves);
	}
	private void MoveChessMan(int X, int Y)
	{
		if(AllowedMoves[X, Y])
		{
			ChessMan C = ChessMans[X, Y];
			if(C != null && C.IsWhite != IsWhiteTurn)
			{
				if(C.GetType() == typeof(King))
				{
					EndGame();
					return;
				}
				ActiveChessMan.Remove(C.gameObject);
				Destroy(C.gameObject);
			}
			ChessMans[SelectedChessMan.CurrentX, SelectedChessMan.CurrentY] = null; //Обнуляем значение предыдущей клетки
			SelectedChessMan.transform.position = GetTileCenter(X, Y); //Перемещаем фигуру на новую клетку
			SelectedChessMan.SetPosition(X, Y);
			ChessMans[X, Y] = SelectedChessMan; //Присваиваем новой клетке фигуру
			IsWhiteTurn = !IsWhiteTurn; //Передаем ход противоположному игроку
		}
		BoardHighLights.Instance.HideHighLights();
		SelectedChessMan = null; //Обнуляем выделение фигуры
	}
	private void UpdateSelection()
	{
		if(!Camera.main) return;
		RaycastHit Hit;
		if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit, 25.0f, LayerMask.GetMask("ChessPlane")))
		{
			SelectionX = (int)Hit.point.x;
			SelectionY = (int)Hit.point.z;
		}
		else SelectionX = SelectionY = -1;
	}
	private void SpawnChessMan(int Index, int X, int Y)
	{
		Vector3 Position = GetTileCenter(X, Y);
		Quaternion Orientation = Quaternion.Euler(0, 0, 0);
		if(Index == 0 || Index == 6) Position.y += 0.33f;
		else if(Index == 3 || Index == 9)
		{
			Position.y += 0.55f;
			Orientation = Quaternion.Euler(90, 0, 0);
		}
		else if(Index == 4) Orientation = Quaternion.Euler(0, 90, 0);
		else if(Index == 5 || Index == 11) Orientation = Quaternion.Euler(90, 0, 0);
		else if(Index == 10) Orientation = Quaternion.Euler(0, -90, 0);

		GameObject ChessGO = Instantiate(ChessManPrefabs[Index], Position, Orientation) as GameObject;
		ChessGO.transform.SetParent(transform);

		ChessMans[X, Y] = ChessGO.GetComponent<ChessMan>();
		ChessMans[X, Y].SetPosition(X, Y);
		ActiveChessMan.Add(ChessGO);
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

		for(int i = 0; i < 8; i++)
		{
			SpawnChessMan(5, i, 1);
			SpawnChessMan(11, i, 6);
		}
	}
	private Vector3 GetTileCenter(int X, int Y)
	{
		Vector3 Origin = Vector3.zero;
		Origin.x += (TILE_SIZE * X) + TILE_OFFSET;
		Origin.z += (TILE_SIZE * Y) + TILE_OFFSET;
		return Origin;
	}
	private void DrawChessBoard()
	{
		Vector3 WidthLine = Vector3.right * 8;
		Vector3 HeightLine = Vector3.forward * 8;

		for(int i = 0; i <= 8; i++) 
		{
			Vector3 Start = Vector3.forward * i;
			Debug.DrawLine(Start, Start + WidthLine);
			Start = Vector3.right * i;
			Debug.DrawLine(Start, Start + HeightLine);
		}
		if(SelectionX >= 0 && SelectionY >= 0)
		{
			Debug.DrawLine(Vector3.forward * SelectionY + Vector3.right * SelectionX, Vector3.forward * (SelectionY + 1) + Vector3.right * (SelectionX + 1));
			Debug.DrawLine(Vector3.forward * (SelectionY + 1) + Vector3.right * SelectionX, Vector3.forward * SelectionY + Vector3.right * (SelectionX + 1));
		}
	}

	private void EndGame()
	{
		if(IsWhiteTurn) Debug.Log("White Team Wins");
		else Debug.Log("Black Team Wins");

		foreach(GameObject GO in ActiveChessMan) Destroy(GO);
		IsWhiteTurn = true;
		BoardHighLights.Instance.HideHighLights();
		SpawnAllChessMan();
	}
}