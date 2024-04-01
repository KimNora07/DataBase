using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;


[System.Serializable]
public class Item
{
    public string type, name, description, number;
    public bool isUsing;

    // GameManager의 itemList안에 데이터를 넣어주기 위한 생성자
    public Item(string type, string name, string description, string number, bool isUsing)
    {
        this.type = type;
        this.name = name;
        this.description = description;
        this.number = number;
        this.isUsing = isUsing;
    }
}

public class GameManager : MonoBehaviour
{
    public TextAsset itemData;
    public List<Item> itemList, myItemList, curItemList;

    public GameObject[] slots;

    public Button rBtn;
    public Button r2Btn;

    public string curType;

    private void Start()
    {
        rBtn.onClick.AddListener(delegate { TapClick("Character"); });
        r2Btn.onClick.AddListener(delegate { TapClick("Balloon"); });

        string[] line = itemData.text.Substring(0, itemData.text.Length - 1).Split('\n');
        print(line.Length);

        for(int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            itemList.Add(new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE"));
        }

        Load();
        Debug.LogFormat("<color=yellow>불러왔습니다</color>");
    }

    public void TapClick(string tabName)
    {
        // 현재 아이템 리스트에 클릭한 타입만 추가
        curType = tabName;
        curItemList = myItemList.FindAll(x => x.type == tabName);

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SetActive(i < curItemList.Count);
            slots[i].GetComponentInChildren<Text>().text = i < curItemList.Count ? curItemList[i].name : "";
        }
    }

    private void Save()
    {
        string data = JsonConvert.SerializeObject(itemList);
        File.WriteAllText(Application.dataPath + "/Resources/MyItemText.txt", data);
    }

    private void Load()
    {
        string data = File.ReadAllText(Application.dataPath + "/Resources/MyItemText.txt");
        myItemList = JsonConvert.DeserializeObject<List<Item>>(data);

        TapClick(curType);
    }

    private void OnApplicationQuit()
    {
        Save();
        Debug.LogFormat("<color=green>저장했습니다</color>");
    }
}
