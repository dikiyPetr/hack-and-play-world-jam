using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterController humanController;
    [SerializeField] public CharacterController demonController;
    [SerializeField] public TilemapPresenter tilemapPresenter;

    public Map map => tilemapPresenter.map;
    public Character human => humanController.character;
    public Character demon => demonController.character;
}