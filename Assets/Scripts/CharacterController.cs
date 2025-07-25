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
    public float moveDuration;
    private bool isMoved = false;

    public async UniTask MoveTo(Vector3 target)
    {
        if (isMoved) return;
        isMoved = true;

        Vector3 start = transform.position;
        float time = 0f;

        while (time < moveDuration)
        {
            float t = time / moveDuration;
            transform.position = Vector3.Lerp(start, target, t);
            await UniTask.Yield();
            time += Time.deltaTime;
        }

        transform.position = target;
        isMoved = false;
    }    
    public void TeleportTo(Vector3 target)
    {
        transform.position = target;
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