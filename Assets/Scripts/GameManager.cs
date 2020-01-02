using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Entity> entities;

    private int currentTurn = 0, currentAction = 0;
    private float amountOfTurnsTaken = 0f;

    private void Start()
    {
        entities = new List<Entity>(FindObjectsOfType<Entity>());
        SortEntities();

        InvokeTurn();
    }

    public void SortEntities()
    {
        for (int i = 0; i < entities.Count - 1; i++)
        {
            for (int j = 0; j < entities.Count - 1; j++)
            {
                Entity currentEntity = entities[j];
                Entity nextEntity = entities[j + 1];

                if (currentEntity.init < nextEntity.init || (currentEntity.init == nextEntity.init && nextEntity is Adventurer || nextEntity is Guardian))
                {
                    entities[j] = nextEntity;
                    entities[j + 1] = currentEntity;
                }
            }
        }
    }

    public void AddEntity(Entity e)
    {
        entities.Add(e);
        SortEntities();
    }

    public void RemoveEntity(Entity e)
    {
        entities.Remove(e);
    }

    public void InvokeTurn()
    {
        amountOfTurnsTaken += 0.5f;
        if (amountOfTurnsTaken <= 5f)
        {
            if (currentAction == 0)
            {
                entities[currentTurn].MovementTurn();
            }
            else
            {
                entities[currentTurn].AttackTurn();
            }
        }
    }

    public void NextTurn()
    {
        currentAction++;

        if (currentAction > 1)
        {
            currentAction = 0;
            currentTurn++;

            if (currentTurn >= entities.Count)
            {
                currentTurn = 0;
            }
        }

        InvokeTurn();
    }
}
