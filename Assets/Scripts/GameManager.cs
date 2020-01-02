﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private List<Entity> entities;

    private int currentEntityTurn = 0, currentAction = 0;
    private float amountOfTurnsTaken = 0f;

    private void Awake()
    {
        instance = this;
    }

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
                entities[currentEntityTurn].MovementTurn();
            }
            else
            {
                entities[currentEntityTurn].AttackTurn();
            }
        }
    }

    public void NextTurn()
    {
        currentAction++;

        if (currentAction > 1)
        {
            currentAction = 0;
            currentEntityTurn++;

            if (currentEntityTurn >= entities.Count)
            {
                currentEntityTurn = 0;
            }
        }

        InvokeTurn();
    }

    public void OnObjectMouseEnter(Object o)
    {
        if (entities[currentEntityTurn].CompareTag("Player"))
        {
            o.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void OnObjectMouseExit(Object o)
    {
        o.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void OnObjectMouseDown(Object o)
    {
        Entity currentEntity = entities[currentEntityTurn];

        if (currentEntity.CompareTag("Player"))
        {
            if (currentAction == 0)
            {
                if (o.CompareTag("Walkable"))
                {
                    if (currentEntity.CanMoveTo(o.Vector2Position))
                    {
                        currentEntity.MoveTo(o.Vector2Position);
                    }
                    else
                    {
                        Vector2 rotateTo = (o.Vector2Position - currentEntity.Vector2Position).normalized;

                        if (currentEntity.CanRotateTo(rotateTo))
                        {
                            currentEntity.RotateTo(rotateTo);
                        }
                    }
                }
            }
            else
            {
                if (o is Entity)
                {
                    if (currentEntity.CanAttack(o.Vector2Position))
                    {
                        currentEntity.Attack(o.GetComponent<Entity>());
                    }
                }
            }
        }
    }

    public static Vector2 AbsVector2(Vector2 v)
    {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }

    public static Vector2 FlipVector2(Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }
}
