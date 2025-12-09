using Assets.Scripts.VR.Core;
using System;
using System.IO;
using TMPro;
using UnityEngine;

public class ControllerActions : MonoBehaviour
{
    public LeftControllerEvents LeftControllerEvents;
    public RightControllerEvents RightControllerEvents;

    public GameObject PopUpWindow;
    private bool _isPopUpWindowVisible = true;
    public GameObject OutputLabelScore;

    public GameObject GameWindow;
    private MiniGame _game;

    public GameObject InputField;

    public GameObject Option1;
    public GameObject Option2;
    public GameObject Option3;
    private int _selectedIndex = 0;

    public GameObject Table;
    public GameObject TableEntry;

    public void Awake()
    {
        _game = GameWindow.GetComponent<MiniGame>();
        RightControllerEvents.OnButtonADown += TogglePopUpWindowVisibility;
        _game.OnGameFinished += HandleGameFinished;
    }

    private void TogglePopUpWindowVisibility()
    {
        _isPopUpWindowVisible = !_isPopUpWindowVisible;
        PopUpWindow.SetActive(_isPopUpWindowVisible);
    }

    public void OnSelectedOptionChanged(int index)
    {
        _selectedIndex = index;
    }

    public void SpawnSelectedObject()
    {
        switch (_selectedIndex)
        {
            case 0:
                Instantiate(Option1, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(Option2, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(Option3, transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    public void OnButtonPlayClick()
    {
        GameWindow.SetActive(true);
        _game.StartGame();
    }

    public void OnButtonImportToTableClick()
    {
        for (int i = 2; i < Table.transform.childCount; i++)
        {
            Destroy(Table.transform.GetChild(i).gameObject);
        }

        using (var fs = File.Open("./TableData.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var sr = new StreamReader(fs))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                var data = line.Split(',');
                var input = data[0];
                var score = Convert.ToInt32(data[1]);

                AddEntryToTable(input, score, false);
            }
        }
    }

    public void OnButtonClearTableClick()
    {
        for (int i = 2; i < Table.transform.childCount; i++)
        {
            Destroy(Table.transform.GetChild(i).gameObject);
        }
    }

    public void OnButtonAddClick()
    {
        var input = InputField.GetComponent<TMP_InputField>().text;
        var score = Convert.ToInt32(OutputLabelScore.GetComponent<TextMeshProUGUI>().text);
        AddEntryToTable(input, score, true);
    }

    public void OnButtonClearClick()
    {
        var input = InputField.GetComponent<TMP_InputField>().text;
        OutputLabelScore.GetComponent<TextMeshProUGUI>().text = "-1";
    }

    private void HandleGameFinished(int finalScore)
    {
        GameWindow.SetActive(false);
        OutputLabelScore.GetComponent<TextMeshProUGUI>().text = finalScore.ToString();
    }

    private void AddEntryToTable(string input, int score, bool isWriteToFile)
    {
        if (string.IsNullOrWhiteSpace(input) || score < 0)
            return;

        var copy = Instantiate(TableEntry, Table.transform);
        var copyLabelName = copy.transform.Find("Content/Background/Elements/LabelName/LabelNameValue").gameObject;
        copyLabelName.GetComponent<TextMeshProUGUI>().text = input;
        var copyLabelScore = copy.transform.Find("Content/Background/Elements/LabelScore/LabelScoreValue").gameObject;
        copyLabelScore.GetComponent<TextMeshProUGUI>().text = score.ToString();
        copy.SetActive(true);

        if (isWriteToFile)
        {
            using (var fs = File.Open("./TableData.txt", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine($"{input},{score}");
            }
        }
    }
}