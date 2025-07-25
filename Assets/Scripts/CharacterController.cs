using System;
using Cysharp.Threading.Tasks;
using Data;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEditor;
using VContainer;

public class CharacterController : MonoBehaviour
{
    public Character character { get; private set; }
    [Inject] private GameManager gameManager;

    private void Awake()
    {
        character = new Character(this, gameManager);
    }

    public Vector2Int debugTarget;
    public float moveSpeed;
    private bool isMoved = false;

    public async UniTask MoveTo(Vector3 target)
    {
        if (isMoved) return;
        isMoved = true;
        for (int i = 0; i < 100; i++)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed);
            await UniTask.DelayFrame(1);
            Debug.Log($"running {(target - transform.position).sqrMagnitude}");
        }

        transform.position = target;
        isMoved = false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterController))]
public class CharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterController controller = (CharacterController)target;
        if (GUILayout.Button("Move To"))
        {
            controller.character.MoveTo(controller.debugTarget);
        }
    }
}
#endif