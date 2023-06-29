using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class Grid2D<T>
{
	public readonly int Width;
	public readonly int Height;
	public readonly float CellSize;

	public Vector2 Position => _position;
	Vector2 _position;

	public Vector2 Pivot => _pivot;
	Vector2 _pivot;

	public Vector2 PivotPosition => _pivotPosition;
	Vector2 _pivotPosition;

	T[,] _cells;

	public Grid2D(Vector2 position, int width, int height, float cellSize, Func<int, int, T> creationFunc) :
		this(position, 0.5f * Vector2.one, width, height, cellSize, creationFunc)
	{
	}

	public Grid2D(Vector2 position, Vector2 pivot, int width, int height, float cellSize, Func<int, int, T> creationFunc)
	{
		Width = width;
		Height = height;
		CellSize = cellSize;
		_pivot = pivot;
		_pivotPosition = new Vector2(Width, Height) * CellSize * _pivot;
		_position = position - _pivotPosition;

		_cells = new T[Width, Height];
		InitializeCells(creationFunc);
	}

	async void InitializeCells(Func<int, int, T> creationFunc)
	{
		await UniTask.NextFrame();

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				_cells[x, y] = creationFunc.Invoke(x, y);
			}
		}
	}

	public T this[Vector2 position]
	{
		get
		{
			GetLocalPosXY(position, out int x, out int y);
			return GetCell(x, y);
		}
	}
	public T this[Vector3 position]
	{
		get
		{
			GetLocalPosXY(position, out int x, out int y);
			return GetCell(x, y);
		}
	}
	public T this[int x, int y] => GetCell(x, y);

	public bool SetCell(Vector2 position, T cell)
	{
		GetLocalPosXY(position, out int x, out int y); 
		if (!InsideGrid(x, y)) return false;

		_cells[x, y] = cell;
		return true;
	}

	public T GetCell(int x, int y)
	{
		if (!InsideGrid(x, y)) return default;
		return _cells[x, y];
	}

	public Vector2 SnapToGrid(Vector2 position)
	{
		GetLocalPosXY(position, out int x, out int y);
		return GetGlobalPos(x, y);
	}

	public Vector2 GetGlobalPos(int x, int y)
	{
		return CellSize * new Vector2(x, y) + _position;
	}

	public void GetLocalPosXY(Vector2 position, out int x, out int y)
	{
		Vector2 localPos = position - _position;
		x = Mathf.FloorToInt(localPos.x / CellSize);
		y = Mathf.FloorToInt(localPos.y / CellSize);
	}

	public void GetLocalPosXY(Vector2Int position, out int x, out int y)
	{
		GetLocalPosXY(position.ToVector2(), out x, out y);
	}

	public Vector2Int GetLocalPos(Vector2 position)
	{
		GetLocalPosXY(position, out int x, out int y);
		return new Vector2Int(x, y);
	}

	public bool InsideGrid(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height;
	}
}
