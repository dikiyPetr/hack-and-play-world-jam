using Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public CharacterController humanController;
    [SerializeField] public CharacterController demonController;
    [SerializeField] public TilemapPresenter tilemapPresenter;
    [SerializeField] public GameState gameState;
    [SerializeField] public AudioManager audioManager;
    public Map map => tilemapPresenter.map;
}