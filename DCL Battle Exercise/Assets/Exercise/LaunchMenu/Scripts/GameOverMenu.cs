using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI armyWins;
    public Button goToMenu;

    public void Populate()
    {
        if ( BattleInstantiator.instance.army1.GetUnits().Count == 0 )
        {
            armyWins.text = "Army 1 wins!";
        }

        if ( BattleInstantiator.instance.army2.GetUnits().Count == 0 )
        {
            armyWins.text = "Army 2 wins!";
        }
    }

    void Awake()
    {
        goToMenu.onClick.AddListener( GoToMenu );
    }

    void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}