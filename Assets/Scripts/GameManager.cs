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
    public Sprite skipCursor;

    public Slider manaBar;
    public Transform hover, pointer;

    public List<Entity> entities;
    public List<Entity> players;

    private Object currentHover;

    private int currentEntityTurn = 0, currentAction = 0;
    private float amountOfTurnsTaken = 0f;

    private CameraMovement cameraMovement;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        entities = new List<Entity>(FindObjectsOfType<Entity>());
        SortEntities();

        foreach (Entity e in entities)
        {
            if (e.CompareTag("Player"))
            {
                players.Add(e);
            }
        }

        cameraMovement = FindObjectOfType<CameraMovement>();
        cameraMovement.GoToPosition(entities[0].transform.position);

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

    private void UpdateCursor(Object hoverObject)
    {
        if (hoverObject == null || !entities[currentEntityTurn].CompareTag("Player"))
        {
            SetCursor(null);
        }
        else
        {
            Entity currentEntity = entities[currentEntityTurn];

            hover.gameObject.SetActive(true);
            hover.position = hoverObject.transform.position;

            if (currentEntity.Equals(hoverObject))
            {
                SetCursor(skipCursor);
            }
            else if (currentAction == 0)
            {
                if (hoverObject.CompareTag("Walkable") && currentEntity.CanMoveTo(hoverObject.Vector2Position))
                {
                    SetCursor(moveCursor);
                }
                else if (currentEntity.CanRotateTo((hoverObject.Vector2Position - currentEntity.Vector2Position).normalized))
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
                if (hoverObject is Entity && currentEntity.CanAttack(hoverObject.Vector2Position))
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

    public bool IsEntityAtPosition(Vector2 position)
    {
        foreach (Entity e in entities)
        {
            if (e.Vector2Position.Equals(position))
                return true;
        }

        return false;
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

        Entity currentEntity = entities[currentEntityTurn];
        pointer.SetParent(currentEntity.transform);
        pointer.position = currentEntity.Vector2Position + Vector2.up * 0.8f;

        if (currentAction == 0)
        {
            if (currentEntity.CompareTag("Player"))
                cameraMovement.trackedObject = currentEntity.transform;

            currentEntity.MovementTurn();
        }
        else
        {
            currentEntity.AttackTurn();
        }
    }

    private IEnumerator NextTurnDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentAction++;

        if (currentHover != null)
            UpdateCursor(currentHover.GetComponent<Object>());

        if (currentAction > 1)
        {
            currentAction = 0;
            currentEntityTurn++;

            if (currentEntityTurn >= entities.Count)
            {
                currentEntityTurn = 0;
            }
        }

        if (entities[currentEntityTurn].isDead)
        {
            StartCoroutine(NextTurnDelayed(0));
            yield break;
        }

        InvokeTurn();
    }

    public void NextTurn()
    {
        StartCoroutine(NextTurnDelayed(0.01f));
    }

    public void OnObjectMouseEnter(Object o)
    {
        currentHover = o;
        UpdateCursor(o);
    }

    public void OnObjectMouseExit(Object o)
    {
        hover.gameObject.SetActive(false);
        currentHover = null;
        UpdateCursor(currentHover);
    }

    public void OnObjectMouseDown(Object o)
    {
        Entity currentEntity = entities[currentEntityTurn];

        if (currentEntity.Equals(o))
        {
            NextTurn();
        }
        else if (currentEntity.CompareTag("Player"))
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

    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static Vector2 ExtremeCeilVector2(Vector2 v)
    {
        return new Vector2
            ((v.x > 0) ? Mathf.Ceil(v.x) : (v.x < 0) ? Mathf.Floor(v.x) : 0,
            (v.y > 0) ? Mathf.Ceil(v.y) : (v.y < 0) ? Mathf.Floor(v.y) : 0);
    }

    public void UpdateManaBar(int newMana)
    {
        manaBar.value = newMana;
    }
}
