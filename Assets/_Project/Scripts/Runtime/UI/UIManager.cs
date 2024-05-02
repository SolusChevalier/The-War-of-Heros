using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public GameObject redCan, blueCan;
    public TeamManager RedTeam, BlueTeam;
    public TextMeshProUGUI RedTeamText, BlueTeamText;

    public void Start()
    {
        RedTeamText.color = Color.red;
        BlueTeamText.color = Color.cyan;
        BlueTeamText.text = "Blue Team\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nUnits: " + RedTeam.UnitCount;
    }

    private void Update()
    {
        if (RedTeam.isTurn)
        {
            redCan.SetActive(true);
            blueCan.SetActive(false);
        }
        if (BlueTeam.isTurn)
        {
            redCan.SetActive(false);
            blueCan.SetActive(true);
        }
        BlueTeamText.text = "Blue Team\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nUnits: " + RedTeam.UnitCount;
    }
}