using NaughtyAttributes;
using UnityEngine;

namespace Source.Game.Scripts
{
    public class SceneLoaderService : MonoBehaviour, IService
    {
        [Scene] [SerializeField] private int _mainMenuScene;
        [Scene] [SerializeField] private int _gamePlayScene;

        public int MainMenuScene => _mainMenuScene;
        public int GamePlayScene => _gamePlayScene;
    }
}