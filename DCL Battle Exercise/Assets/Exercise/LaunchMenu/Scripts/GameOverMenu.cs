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
            if (BattleInstantiator.instance.Army1.Units.Count == 0)
            {
                armyWins.text = "Army 1 wins!";
            }

            if (BattleInstantiator.instance.Army2.Units.Count == 0)
            {
                armyWins.text = "Army 2 wins!";
            }
        }

        void Awake() => goToMenu.onClick.AddListener(GoToMenu);

        void GoToMenu() => SceneManager.LoadScene(0);
    }
}