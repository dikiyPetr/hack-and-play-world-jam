using System.Runtime.InteropServices;
using Data;
using UnityEditor.Tilemaps;
using UnityEngine;
using VContainer;

public class Character
{
    private CharacterController _controller;
    private GameManager _gameManager;
  
    public Character(CharacterController controller, GameManager gameManager)
    {
        _controller = controller;
        _gameManager = gameManager;
    }

    public void MoveTo(Vector2Int target)
    {
        var position = _gameManager.tilemapPresenter.GetTilePosition(target);
        _controller.MoveTo(position);

    }
}