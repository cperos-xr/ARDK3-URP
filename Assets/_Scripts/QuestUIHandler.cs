using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private GameObject content;

    [SerializeField] private List<InventoryButton> buttonObjects;

    //private Dictionary <IButtonObject, InventoryButton> iButtonDictionary = new Dictionary<IButtonObject, InventoryButton>();


    // Start is called before the first frame update

    private void OnEnable()
    {
        QuestManager.OnPlayerAssignedQuest += HandleAssignedPlayerQuest;
        CheckQuests();
    }

    public void CheckQuests()
    {
        foreach (InventoryButton button in buttonObjects)
        {
            // Null-check for 'button' and 'button.buttonText'
            if (button?.buttonText == null)
            {
                // Log or throw exception
                Debug.LogError("Button or buttonText is null.");
                continue;
            }

            if (button.iButtonObject is SO_Task task)
            {
                // Assume TaskManager.Instance is a singleton and Instance can't be null; otherwise, null-check.
                bool isTaskCompleted = TaskManager.Instance.IsTaskCompleted(task);

                // Using the ternary operator for brevity.
                button.buttonText.color = isTaskCompleted ? Color.green : Color.black;
                button.icon.color = isTaskCompleted ? Color.gray: Color.white;

                ColorBlock cb = button.button.colors;
                cb.normalColor = isTaskCompleted ? Color.blue : Color.white;
                button.button.colors = cb;
            }
            else if (button.iButtonObject == null)
            {
                // Log or throw exception
                Debug.LogError("InventoryObject is null.");
            }
            else
            {
                // Log or throw exception
                Debug.LogError("InventoryObject is not of type SO_Task.");
            }

            Debug.Log("iButtonObject " + button.iButtonObject.ObjectName);
        }
    }

    private void OnDisable()
    {
        QuestManager.OnPlayerAssignedQuest -= HandleAssignedPlayerQuest;
    }

    private void HandleAssignedPlayerQuest(SO_Quest quest)
    {
        foreach (var task in quest.tasks) 
        {
            MakeNewButton(task);
        }
    }

    public void MakeNewButton(SO_Task task)
    {
        Debug.Log("making new Button");
        GameObject newTaskButton = Instantiate(buttonTemplate, content.transform);
        InventoryButton taskButton = newTaskButton.GetComponent<InventoryButton>();

        taskButton.InitialIzeItemButton(task);

        newTaskButton.SetActive(true);
        buttonObjects.Add(taskButton);
        //iButtonDictionary.Add(task, taskButton);
        
        Debug.Log("Created inventory item", newTaskButton);
    }
}
