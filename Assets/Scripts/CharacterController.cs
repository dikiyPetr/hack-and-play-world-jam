using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEditor;

public class CharacterController : MonoBehaviour
{
    public Vector2Int debugTarget;
    public float moveDuration;
    public bool isMoved = false;
    public bool isHuman;
    public TMP_Text textObject;
    public GameObject textPanel;


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

    public async void Say(String text)
    {
        textObject.SetText(text);
        textPanel.SetActive(true);
        await UniTask.Delay(6000);
        textPanel.SetActive(false);
    }

    public void HideSay()
    {
        textPanel.SetActive(false);
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
        // if (GUILayout.Button("Move To"))
        // {
        //     controller.character.MoveTo(controller.debugTarget);
        // }
    }
}
#endif