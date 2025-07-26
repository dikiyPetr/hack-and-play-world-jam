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
            switch (type)
            {
                case StopGameType.None:
                    break;
                case StopGameType.Finish:
                    _label.text = StringAssets.FinishLevelLabel;
                    break;
                case StopGameType.DeathByLava:
                    _label.text = StringAssets.DeathByLavaLabel;
                    break;
                case StopGameType.DeathByWater:
                    _label.text = StringAssets.DeathByWaterLabel;
                    break;
                case StopGameType.DeathByRats:
                    _label.text = StringAssets.DeathByRatsLabel;
                    break;
                case StopGameType.Merge:
                    _label.text = StringAssets.DeathByMergeLabel;
                    break;
                case StopGameType.Recursion:
                    _label.text = "AAAAA. МЫ ДОБАВЛЯЕМ КОД, КОТОРЫЙ ПОТОМ НЕ ИСПОЛЬЗУЕМ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ActivateNextLvl()
        {
            _nextLvlButton.SetEnabled(true);
        }
    }
}