using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;// 바라보는 위치 일치하게 함
    public Vector3 playerRot;// 바라보는 방향까지 일치하게 함

    public List<int> invenArrayNumber = new List<int>();
    public List<string> inventoryName = new List<string>();
    public List<int> invenItemNumber = new List<int>();
}

public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILE_NAME = "/SaveFile.txt";

    private PlayerController theplayer;

    private Inventory theInventory;

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 경로가 존재하지 않으면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void SaveData()
    {
        theplayer = FindObjectOfType<PlayerController>();
        theInventory = FindObjectOfType<Inventory>();

        saveData.playerPos = theplayer.transform.position;
        saveData.playerRot = theplayer.transform.eulerAngles;

        Slot[] slots = theInventory.GetSlots();

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.inventoryName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME, json); // 실제물리적인 파일에 json 형식으로 저장

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME); //json파일을 받아 saveData에 저장
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            theplayer = FindObjectOfType<PlayerController>();
            theInventory = FindObjectOfType<Inventory>();

            theplayer.transform.position = saveData.playerPos;
            theplayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemNumber.Count; i++)
            {
                theInventory.LoadToInven(saveData.invenArrayNumber[i],
                    saveData.inventoryName[i],
                    saveData.invenItemNumber[i]);
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
    }
}
