using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Image customCursor;
    public Sprite prohibitedCursor;
    public Sprite moveCursor;
    public Sprite rotateCursor;
    public Sprite attackCursor;

    public Slider manaBar;
    public Transform hover;

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

    private void Update()
    {
        customCursor.transform.position = Input.mousePosition;
    }

    private void SetCursor(Sprite cursor)
    {
        if (cursor == null)
        {
            Cursor.visible = true;
            customCursor.gameObject.SetActive(false);
        }
        else
        {
            Cursor.visible = false;
            customCursor.gameObject.SetActive(true);
            customCursor.sprite = cursor;
        }
    }

    public void SortEntities()
    {
        for (int i = 0; i < entities.Count - 1; i++)
        {
            for (int j = 0; j < entities.Count - 1; j++)
            {
                Entity currentEntity = entities[j];
                Entity nextEntity = entities[j + 1];

                if (currentEntity.init < nextEntity.init || (currentEntity.init == nextEntity.init && (nextEntity is Adventurer || nextEntity is Guardian)))
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
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

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
    public void SkipTurn()
    {
        NextTurn();
    }

    public void OnObjectMouseEnter(Object o)
    {
        if (entities[currentEntityTurn].CompareTag("Player"))
        {
            Entity currentEntity = entities[currentEntityTurn];

            hover.gameObject.SetActive(true);
            hover.position = o.transform.position;

            if (currentAction == 0)
            {
                if (currentEntity.CanMoveTo(o.Vector2Position))
                {
                    SetCursor(moveCursor);
                }
                else if (currentEntity.CanRotateTo((o.Vector2Position - currentEntity.Vector2Position).normalized))
                {
                    SetCursor(rotateCursor);
                }
                else
                {
                    SetCursor(prohibitedCursor);
                }
            }
            else
            {
                if (o is Entity && currentEntity.CanAttack(o.Vector2Position))
                {
                    SetCursor(attackCursor);

                }
                else
                {
                    SetCursor(prohibitedCursor);
                }
            }
        }
    }

    public void OnObjectMouseExit(Object o)
    {
        hover.gameObject.SetActive(false);
        SetCursor(null);
    }

    public void OnObjectMouseDown(Object o)
    {
        Entity currentEntity = entities[currentEntityTurn];

        if (currentEntity.CompareTag("Player"))
        {
            if (currentAction == 0)
            {
                if (!currentEntity.CanMoveTo(o.Vector2Position))
                {
                    Vector2 rotateTo = (o.Vector2Position - currentEntity.Vector2Position).normalized;

                    if (currentEntity.CanRotateTo(rotateTo))
                    {
                        currentEntity.RotateTo(rotateTo);
                    }
                }
                else
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
    public void UpdateManaBar(int newMana)
    {
        manaBar.value = newMana;
    }
}
