using TMPro;
using UnityEngine;

public class ShowComponentsOnHover : MonoBehaviour
{
    public GameObject HoverWindow;
    public GameObject Table;
    public GameObject TableEntry;

    public void ShowWindow(Transform target)
    {
        for (int i = 2; i < Table.transform.childCount; i++)
        {
            Destroy(Table.transform.GetChild(i).gameObject);
        }

        HoverWindow.SetActive(true);
        HoverWindow.transform.position = new Vector3(target.position.x, target.position.y, target.position.z + 0.25f);

        foreach (var it in target.GetComponents(typeof(Component)))
        {
            var copy = Instantiate(TableEntry, Table.transform);
            var copyLabelName = copy.transform.Find("Content/Background/Elements/LabelName/LabelNameValue").gameObject;
            copyLabelName.GetComponent<TextMeshProUGUI>().text = it.GetType().Name;
            copy.SetActive(true);
        }
    }
}