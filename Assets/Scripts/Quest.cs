using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public List<QuestObjective> objectives;

    //Called when scriptable object is edited
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }

}
    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveID; //Match with item ID that you need to collect, enemy ID to defeat, etc.
        public string description;
        public ObjectiveType type; //Enum to define the type of objective
        public int requiredAmount;
        public int currentAmount;
        public bool isCompleted => currentAmount >= requiredAmount;
    }
    public enum ObjectiveType { CollectItem, ReachLocation, DefeatEnemy, TalkNPC, Custom }
    /* ExploreArea, EscortNPC, UseItem, 
     CraftItem, 
     SolvePuzzle, CompleteChallenge, 
     GatherResource, DefendLocation, 
     InteractWithObject, 
     FindHiddenObject, 
     ParticipateInEvent, 
     CompleteQuestLine}*/

    [System.Serializable]
    public class QuestProgress
    {
        public Quest quest;
        public List<QuestObjective> objectives;

        public QuestProgress(Quest quest)
        {
            this.quest = quest;
            objectives = new List<QuestObjective>();

            foreach (var obj in quest.objectives)
            {
                objectives.Add(new QuestObjective
                {
                    objectiveID = obj.objectiveID,
                    description = obj.description,
                    type = obj.type,
                    requiredAmount = obj.requiredAmount,
                    currentAmount = 0 // Start with 0 progress
                });
            }
        }

        public bool IsCompleted => objectives.TrueForAll(o => o.isCompleted);
        public string QuestID => quest.questID;

    }
