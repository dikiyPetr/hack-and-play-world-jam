using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharacterController humanController;
    [SerializeField] private CharacterController demonController;
    [SerializeField] public TilemapPresenter tilemapPresenter;

    public Map map => tilemapPresenter.map;
    public Character human => humanController.character;
    public Character demon => demonController.character;
}