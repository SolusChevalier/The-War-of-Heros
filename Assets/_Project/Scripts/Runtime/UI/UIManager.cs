using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    //simple ui manager
    public GameObject redCan, blueCan;

    public TeamManager RedTeam, BlueTeam;
    public TextMeshProUGUI RedTeamText, BlueTeamText;
    public float redScore, blueScore;

    public void Start()
    {
        RedTeamText.color = Color.red;
        BlueTeamText.color = Color.cyan;
        BlueTeamText.text = "Blue Team\nScore: " + BlueTeam.GetTeamValue() + "\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nScore: " + RedTeam.GetTeamValue() + "\nUnits: " + RedTeam.UnitCount;
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
        BlueTeamText.text = "Blue Team\nScore: " + BlueTeam.GetTeamValue() + "\nUnits: " + BlueTeam.UnitCount;
        RedTeamText.text = "Red Team\nScore: " + RedTeam.GetTeamValue() + "\nUnits: " + RedTeam.UnitCount;
    }
}