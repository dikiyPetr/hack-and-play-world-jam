using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class StopGameUI : MonoBehaviour
    {
        private VisualElement _root;
        private Button _restartButton;
        private Button _nextLvlButton;
        private Label _label;
        [SerializeField] private GameState gameState;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _restartButton = _root.Q<Button>("Restart");
            _nextLvlButton = _root.Q<Button>("NextLvl");
            _label = _root.Q<Label>("Label");
        }


        private void OnEnable()
        {
            _root.visible = false;
            _restartButton.clicked += OnRestartLvl;
            _nextLvlButton.clicked += OnNextLvl;
        }

        private void OnDisable()
        {
            _root.visible = false;
            _restartButton.clicked -= OnRestartLvl;
            _nextLvlButton.clicked -= OnNextLvl;
        }

        private void OnRestartLvl()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }     
        private void OnNextLvl()
        {
            SceneManager.LoadScene(gameState.levelSetup.nextScene.ToString());
        }

        public void Show(StopGameType type)
        {
            _root.visible = true;

            if (type == StopGameType.Recursion)
            {
                _label.text = "Закатался на карусели";
                return;
            }

            if (type == StopGameType.Merge)
            {
                _label.text = "СЛИЯНИЕЕЕЕ!";
                return;
            
            }  
            if (type == StopGameType.Death)
            {
                _label.text = "ЭТО СМЕРТЬ!";
                return;
            
            }  
            if (type == StopGameType.Finish)
            {
                _label.text = "ПОБЕДА!";
                return;
            
            }
        }
        public void ActivateNextLvl()
        {
            _nextLvlButton.SetEnabled(true);
        }
    }
}