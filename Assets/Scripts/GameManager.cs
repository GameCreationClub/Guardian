using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Entity> entities;

    private void Start()
    {
        entities = new List<Entity>(FindObjectsOfType<Entity>());

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
}
