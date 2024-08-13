using Exercise.Battle.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Exercise.LaunchMenu.Scripts
{
    public sealed class GameOverMenu : MonoBehaviour
    {
        public TextMeshProUGUI armyWins;
        public Button goToMenu;

        public void Populate()
        {
            foreach (var army in BattleInstantiator.instance.Armies)
            {
                if (!army.IsDefeated) continue;
                armyWins.text = army.Name + " lost!";
                break;
            }
        }

        private void Awake() => goToMenu.onClick.AddListener(GoToMenu);

        private static void GoToMenu() => SceneManager.LoadScene(0);
    }
}