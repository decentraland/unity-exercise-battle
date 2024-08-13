using System.Collections.Generic;
using System.Linq;
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
        private readonly List<Army> defeatedArmies = new();

        public void Populate(Army army, List<Army> armies)
        {
            if (defeatedArmies.Contains(army)) return;

            defeatedArmies.Add(army);

            var defeatedNames = string.Join(", ", defeatedArmies.Select(army => army.Name));
            armyWins.text = defeatedNames + " defeated! ";
            
            if (defeatedArmies.Count < armies.Count - 1) return;

            var win = armies.First((find) => !defeatedArmies.Contains(find));
            armyWins.text += win.Name + " victory!";
        }

        private void Awake() => goToMenu.onClick.AddListener(GoToMenu);

        private void GoToMenu()
        {
            defeatedArmies.Clear();
            SceneManager.LoadScene(0);
        }
    }
}