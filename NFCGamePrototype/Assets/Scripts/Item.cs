using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public PlayerManager playermanager;

    private Quaternion originalRotation;
    private Vector3 originalPosition;

    public int Mod0;
    public int Mod1;
    public int Mod2;
    public int Mod3;
    public int Mod4;
    public int IndexInInventory;

    public string Type;
    public string Name;
    public string Description;
    public string Effect;

    // "IDENTIFIER(string):MOD#(int, 0-4):STAT(int, 0-6)"
    public string Command;

    // Variables for raise mode interactions
    public bool Tappable = false;
    public bool Draggable = false;
    private Vector3 screenpoint;
    private Vector3 offset;

    //public string[] CommandSequence;

    private void Start()
    {
        playermanager = GameObject.Find("PlayerFigure").GetComponent<PlayerManager>();
        originalRotation = this.transform.rotation;
        originalPosition = this.transform.position;
    }

    private void Update()
    {
        if (playermanager.inventoryItemSelected == this)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 15.0f);
        }
    }

    private void OnTriggerEnter (Collider other)
    {
        Debug.Log("Feeding item to character");
        if (other.gameObject.name == "PlayerFigure")
        {
            Execute();
            playermanager.RemoveItem(IndexInInventory);
            Destroy(this.gameObject);
        }
    }

    void ParseCommands(string[] commands)
    {
        //Todo: multiple command executions
    }

    public void Execute()
    {
        Debug.Log("Executing command");
        Execute(Command);
    }

    void Execute(string command)
    {
        string[] commandList = command.Split(':');
        string id = commandList[0];
        int statIndex = System.Int32.Parse(commandList[2]);
        int statvalue = 0;
        switch (commandList[1]){
            case "0":
                statvalue = Mod0;
                break;
            case "1":
                statvalue = Mod1;
                break;
            case "2":
                statvalue = Mod2;
                break;
            case "3":
                statvalue = Mod3;
                break;
            case "4":
                statvalue = Mod4;
                break;
        }
        // Handle stat updates
        if (id == "UpStat")
        {
            UpdateStat(statIndex, statvalue);
        }
    }

    void UpdateStat(int statIndex, int value)
    {
        playermanager.UpdateStat(statIndex, value);
    }

    public void UpdateItemText()
    {
        playermanager.UpdateItemText(this);
    }

    public void SelectThis()
    {
        Item lastSelected = playermanager.inventoryItemSelected;
        playermanager.inventoryItemSelected = this;
        this.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        if (lastSelected != null)
        {
            lastSelected.transform.rotation = lastSelected.originalRotation;
            lastSelected.transform.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
        }
        UpdateItemText();
    }

    public void PickupState(string state)
    {
        if (Draggable)
        {
            if (state == "pickup")
            {
                this.transform.localScale += new Vector3(0.75f, 0.75f, 0.75f);
            }
            if (state == "dropped")
            {
                this.transform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);
                this.transform.position = originalPosition;
            }
            
        }
    }

    public void PickupItem()
    {
        if (Draggable)
        {
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 positionToMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            transform.position = new Vector3(positionToMove.x, transform.position.y, positionToMove.z);
        }
    }
}
