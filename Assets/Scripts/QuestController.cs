using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<QuestProgress> activeQuests = new();
    private QuestUI questUI;

    public List<string> handinQuestIDs = new(); // List to store quest IDs that can be handed in

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        questUI = FindFirstObjectByType<QuestUI>();
        Inventorycontroller.Instance.OnInventoryChanged += CheckInventoryForQuests; // Subscribe to inventory changes
    }

    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID)) return;

        activeQuests.Add(new QuestProgress(quest));

        CheckInventoryForQuests(); // Check inventory for quest objectives immediately after accepting a quest
        questUI.UpdateQuestUI();
    }

    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.QuestID == questID);

    public void CheckInventoryForQuests()
    {
        Dictionary<int, int> itemCounts = Inventorycontroller.Instance.GetItemCounts();

        foreach (QuestProgress quest in activeQuests)
        {
            foreach (QuestObjective questObjective in quest.objectives)
            {
                if (questObjective.type != ObjectiveType.CollectItem) continue;
                if (!int.TryParse(questObjective.objectiveID, out int itemID)) continue;

                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Math.Min(count, questObjective.requiredAmount) : 0;

                if (questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                    //questUI.UpdateQuestUI(); // Update the UI to reflect the changes
                }
            }

        }
        questUI.UpdateQuestUI(); // Update the UI after checking all quests
    }
    
    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.isCompleted);
    }

    public void HandInQuest(string questID)
    {
        //Try remove required items from inventory
        if (!RemoveRequiredItemsFromInventory(questID))
        {
            //Quest cannot be handed in due to insufficient items
            return;
        }

        //Remove quest from active quests
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        if (quest != null)
        {
            handinQuestIDs.Add(questID); // Add the quest ID to the hand-in list
            activeQuests.Remove(quest);
            questUI.UpdateQuestUI(); // Update the UI after removing the quest
        }

    }

    public bool IsQuestHandedIn(string questID)
    {
        return handinQuestIDs.Contains(questID);
    }

    public bool RemoveRequiredItemsFromInventory(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        if (quest == null) return false;

        Dictionary<int, int> requiredItems = new(); // Dictionary to store required items and their amounts

        //Item requirements from objectives
        foreach (QuestObjective objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.CollectItem && int.TryParse(objective.objectiveID, out int itemID))
            {
                requiredItems[itemID] = objective.requiredAmount; // Store the required amount for this item
            }

        }

        //Verify if we have items
        Dictionary<int, int> itemCounts = Inventorycontroller.Instance.GetItemCounts();
        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                return false; // Not enough items to complete the quest
            }
        }
        //Remove items from inventory
        foreach (var itemRequirement in requiredItems)
        {
            Inventorycontroller.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }
        return true; // Successfully removed all required items
    }

    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activeQuests = savedQuests ?? new();
        CheckInventoryForQuests(); // Check inventory for quest objectives after loading quests
        questUI.UpdateQuestUI(); // Update the UI to reflect the loaded quests
    }
    
}
