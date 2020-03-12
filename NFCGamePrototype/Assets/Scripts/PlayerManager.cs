using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    // The unique identifier of the figure currently being used in-game.
    public string figureID;
    
    // {0=Name(FigureID), 1=Nickname(Player-given), 2=Element}
    private string[] detailsList = new string[3];

    // {0=LVL(Level), 1=XP(Experience), 2=HP(Health), 3=ATK(Attack), 4=MAG(Magic), 5=SPD(Speed), 6=HAP(Happiness)}
    private int[] statsList = new int[7];

    // {0=T(Tokens), 1=O(Orbs)}
    private int[] currencyList = new int[2];
    public string[] inventory = new string[128];

    public ItemDatabase itemDatabase;
    public Canvas sceneUI;
    public GameObject itemGrid;
    public GameObject raiseGrid;

    // Text related to character stats
    public Text DetailsText;
    public Text CurrencyText;
    public Text StatsText;

    // Text related to items (inventory, raising, shop)
    public Text DescriptionText;
    public Text EffectText;
    public Text RaiseItemNameText;
    public Text RaiseItemEffectText;

    // Misc parameters for items & player interactions
    public Item inventoryItemSelected;
    public Item raiseItemSelected;
    public int raiseItemCurrentIndex;

    public void Save()
    {
        if (figureID != null)
        {
            PlayerPrefs.SetString(figureID + "Details", detailsList[0] + ":"
                + detailsList[1] + ":"
                + detailsList[2]);
            PlayerPrefs.SetString(figureID + "Stats", statsList[0] + ":"
                + statsList[1] + ":"
                + statsList[2] + ":"
                + statsList[3] + ":"
                + statsList[4] + ":"
                + statsList[5] + ":"
                + statsList[6]);
            PlayerPrefs.SetString("PlayerCurrency", currencyList[0] + ":" 
                + currencyList[1]);
            PlayerPrefs.SetString("PlayerInventory", string.Join(":", inventory));
        }
    }

    public void Load()
    {
        if (figureID != null)
        {
            // Load in character details
            detailsList = PlayerPrefs.GetString(figureID + "Details").Split(':');

            // Load in character stats
            string[] stats = PlayerPrefs.GetString(figureID + "Stats").Split(':');
            for (int i = 0; i < statsList.Length; i++)
            {
                statsList[i] = System.Int32.Parse(stats[i]);
            }

            // Load in player currencies
            string[] currency = PlayerPrefs.GetString("PlayerCurrency").Split(':');
            for (int i = 0; i < currencyList.Length; i++)
            {
                currencyList[i] = System.Int32.Parse(currency[i]);
            }

            // Load in player inventory
            string[] inv = PlayerPrefs.GetString("PlayerInventory").Split(':');
            for (int i = 0; i < inv.Length; i++)
            {
                if (inv[i] != "")
                {
                    inventory[i] = inv[i];
                }
            }
        }
    }

    public void setupPlayerData(string figID)
    {
        figureID = figID;
        if (PlayerPrefs.GetString(figureID + "Details") == "")
        {
            setupNewChar();
        } else
        {
            Load();
        }
    }

    public void AddItem(string code)
    {
        for(int i=0; i < inventory.Length; i++)
        {
            if (inventory[i] == "")
            {
                inventory[i] = code;
                return;
            }
        }
    }

    public void RemoveItem(int index)
    {
        string[] updatedInventory = new string[128];
        string tabId = inventory[index].Split('-')[0];
        for (int i = 0; i < index; i++)
        {
            updatedInventory[i] = inventory[i];
        }
        for (int j = index+1; j < updatedInventory.Length; j++)
        {
            updatedInventory[j - 1] = inventory[j];
        }
        inventory = updatedInventory;
        //UpdateInventoryDisplay(tabId);
        UpdateRaiseItemDisplay(raiseItemCurrentIndex);
    }

    public void setupNewChar()
    {
        if (figureID !=null){
            detailsList[0] = figureID;
            detailsList[1] = "NoName";
            detailsList[2] = "NoType";

            statsList[0] = 1;
            statsList[1] = 0;
            statsList[2] = 100;
            statsList[3] = 1;
            statsList[4] = 2;
            statsList[5] = 3;
            statsList[6] = 50;

            if (PlayerPrefs.GetString("PlayerCurrency") == "")
            {
                //ToDo: change these both to zero for new players
                currencyList[0] = 10000;
                currencyList[1] = 250;
            }

            /*
            inventory[0] = "I-1";
            inventory[1] = "I-0";
            inventory[2] = "E-0";
            inventory[3] = "I-1";
            inventory[4] = "I-1";
            inventory[5] = "I-0";
            inventory[6] = "I-0";
            inventory[7] = "I-0";
            */

            Save();
        }
    }

    public void updateStatText()
    {
        if (figureID != null)
        {
            DetailsText.text = detailsList[0]
                + "\n(" + detailsList[1] + ")"
                + "\nType: " + detailsList[2];

            CurrencyText.text = "Tokens: " + currencyList[0].ToString()
                + "\nOrbs: " + currencyList[1].ToString();

            StatsText.text = "Level: " + statsList[0].ToString()
                + "\nXP: " + statsList[1].ToString() + "/" + (statsList[0] * 100).ToString()
                + "\nHP: " + statsList[2].ToString()
                + "\nATK: " + statsList[3].ToString()
                + "\nMAG: " + statsList[4].ToString()
                + "\nSPD: " + statsList[5].ToString()
                + "\nHAP: " + statsList[6].ToString();
        }
    }

    public void UpdateItemText(Item item)
    {
        DescriptionText.text = item.Description;
        EffectText.text = item.Effect;
    }

    public void UpdateStat(int index, int value)
    {
        statsList[index] += value;
        updateStatText();
        Save();
    }

    public void UpdateStat(string keyValue)
    {
        string[] values = keyValue.Split(':');
        UpdateStat(System.Int32.Parse(values[0]), System.Int32.Parse(values[1]));

    }

    public void UpdateInventoryDisplay(string tabId)
    {
        ClearShownInventory();
        int inventoryIndex = 0;
        int slotNumber = 0;
        foreach(string code in inventory)
        {
            if (code.Length > 0 && code.Split('-')[0] == tabId)
            {
                GameObject itemObject = itemDatabase.getItem(code);
                itemObject.GetComponent<Item>().IndexInInventory = inventoryIndex;
                Transform slot = itemGrid.transform.Find("slot" + slotNumber.ToString());
                Instantiate(itemObject, slot);
                slotNumber++;
                if (slotNumber == itemGrid.transform.childCount)
                {
                    return;
                }
            }
            inventoryIndex++;
        }
    }

    public void UpdateRaiseStatDisplay()
    {
        Transform statsbar = sceneUI.transform.Find("RaiseUI").Find("StatsBar");
        foreach (Transform t in statsbar)
        {
            string statname = t.name;
            switch (statname)
            {
                case "HP":
                    t.GetComponent<Text>().text = this.statsList[2].ToString();
                    break;
                case "ATK":
                    t.GetComponent<Text>().text = this.statsList[3].ToString();
                    break;
                case "MAG":
                    t.GetComponent<Text>().text = this.statsList[4].ToString();
                    break;
                case "SPD":
                    t.GetComponent<Text>().text = this.statsList[5].ToString();
                    break;
                case "HAP":
                    t.GetComponent<Text>().text = this.statsList[6].ToString();
                    break;
            }
        }
    }

    public void UpdateRaiseItemDisplay(int scrollIndex)
    {
        UpdateRaiseStatDisplay();
        raiseItemCurrentIndex += scrollIndex;
        int inventoryIndex = 0;
        // If index is negative, no more previous items exist ==> stop scrolling.
        if (raiseItemCurrentIndex < 0)
        {
            raiseItemCurrentIndex++;
            return;
        }
        // If index is greater than total number of items, no more items in inventory, ==> stop scrolling.
        if (raiseItemCurrentIndex == TotalItemsOfType("I"))
        {
            raiseItemCurrentIndex--;
            return;
        }
        ClearShownRaiseItems();
        int slotNumber = 0;
        int found = 0;
        foreach (string code in inventory)
        {
            if (code.Length > 0 && code.Split('-')[0] == "I")
            {
                GameObject itemObject = itemDatabase.getItem(code);
                found++;
                if (found >= raiseItemCurrentIndex) // Skips the first xx items according to where user has scrolled.
                {
                    //Special case for first item in the list
                    if (raiseItemCurrentIndex == 0 && slotNumber == 0)
                    {
                        slotNumber = 1;
                    }
                    if (slotNumber == 1)
                    {
                        RaiseItemNameText.text = itemObject.GetComponent<Item>().Name;
                        RaiseItemEffectText.text = itemObject.GetComponent<Item>().Effect;
                        itemObject.GetComponent<Item>().Draggable = true;

                        itemObject.GetComponent<Item>().IndexInInventory = inventoryIndex;
                    }
                    else
                    {
                        itemObject.GetComponent<Item>().Draggable = false;
                    }
                    Transform slot = raiseGrid.transform.Find("slot" + slotNumber.ToString());
                    Instantiate(itemObject, slot);
                    slotNumber++;

                    if (slotNumber == raiseGrid.transform.childCount)
                    {
                        return;
                    }
                }
            }
            inventoryIndex++;
        }
    }

    public void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All PlayerPrefs have been cleared.");
    }

    public void ClearShownInventory()
    {
        foreach (Transform g in itemGrid.transform)
        {
            if (g.childCount > 0)
            {
                GameObject child = g.transform.GetChild(0).gameObject;
                GameObject.Destroy(child);
            }
        }
    }

    public void ClearShownRaiseItems()
    {
        foreach (Transform g in raiseGrid.transform)
        {
            if (g.childCount > 0)
            {
                GameObject child = g.transform.GetChild(0).gameObject;
                GameObject.Destroy(child);
            }
        }
    }

    public int TotalItemsOfType(string tabId)
    {
        int total = 0;
        foreach (string code in inventory)
        {
            if (code != null)
            {
                if (code.Length > 0 && code.Split('-')[0] == tabId)
                {
                    total++;
                }
            }
        }
        return total;
    }
}
