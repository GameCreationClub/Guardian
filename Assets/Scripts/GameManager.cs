using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public GameObject moveMarkerPrefab;
    public GameObject rotateMarkerPrefab;
    public Transform hover, pointer, facingArrow;

    public List<Entity> entities;
    public List<Entity> players;

    public Transform turnOrderParent;
    public GameObject turnImagePrefab;
    public GameObject enemyMoveTo;

    private bool adventurerMovedOnTurn = false;

    private Image[] turnOrderImages;
    private Color
        defaultTurnOrderFrameColor = new Color(0.8196f, 0.8196f, 0.8196f),
        selectedTurnOrderFrameColor = new Color(0.3647f, 0.7372f, 0.8235f);

    private GameObject[] walkables;

    private Object currentHover;

    private Transform[] moveMarkers = new Transform[5];
    private Transform[] rotateMarkers = new Transform[3];

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

        walkables = GameObject.FindGameObjectsWithTag("Walkable");

        cameraMovement = FindObjectOfType<CameraMovement>();
        cameraMovement.GoToPosition(entities[0].transform.position);

        for (int i = 0; i < moveMarkers.Length; i++)
        {
            moveMarkers[i] = Instantiate(moveMarkerPrefab).transform;
            moveMarkers[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < rotateMarkers.Length; i++)
        {
            rotateMarkers[i] = Instantiate(rotateMarkerPrefab).transform;
            rotateMarkers[i].gameObject.SetActive(false);
        }

        InvokeTurn();
        UpdateMoveMarkers();
        UpdateRotateMarkers();
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

    private void ShowMoveMarker(int index, Vector2 position)
    {
        moveMarkers[index].gameObject.SetActive(true);
        moveMarkers[index].position = position;
    }

    private void UpdateMoveMarkers()
    {
        int moveMarkersUsed = 0;

        Entity currentEntity = entities[currentEntityTurn];

        if (currentEntity.CompareTag("Player") && currentAction == 0)
        {
            for (moveMarkersUsed = 0; moveMarkersUsed < currentEntity.init; moveMarkersUsed++)
            {
                Vector2 moveMarkerPosition = currentEntity.Vector2Position + currentEntity.facingDirection * (moveMarkersUsed + 1);

                if (currentEntity.CanMoveTo(moveMarkerPosition))
                    ShowMoveMarker(moveMarkersUsed, moveMarkerPosition);
            }

            Vector2 flippedFacingDirection = FlipVector2(currentEntity.facingDirection);
            Vector2 diagonal1Position = currentEntity.Vector2Position + currentEntity.facingDirection + flippedFacingDirection;
            Vector2 diagonal2Position = currentEntity.Vector2Position + currentEntity.facingDirection + flippedFacingDirection * -1;

            if (currentEntity.CanMoveTo(diagonal1Position))
            {
                ShowMoveMarker(moveMarkersUsed, diagonal1Position);
                moveMarkersUsed++;
            }

            if (currentEntity.CanMoveTo(diagonal2Position))
            {
                ShowMoveMarker(moveMarkersUsed, diagonal2Position);
                moveMarkersUsed++;
            }
        }

        for (int j = moveMarkers.Length - 1; j >= moveMarkersUsed; j--)
        {
            moveMarkers[j].gameObject.SetActive(false);
        }
    }

    private void ShowRotateMarker(int index, Vector2 position)
    {
        rotateMarkers[index].gameObject.SetActive(true);
        rotateMarkers[index].position = position;
    }

    private void UpdateRotateMarkers()
    {
        Entity currentEntity = entities[currentEntityTurn];
        int rotateMarkersUsed = 0;

        if (currentEntity.CompareTag("Player") && currentAction == 0)
        {
            Vector2 flippedFacingDirection = FlipVector2(currentEntity.facingDirection);

            ShowRotateMarker(0, currentEntity.Vector2Position + flippedFacingDirection);
            ShowRotateMarker(1, currentEntity.Vector2Position + flippedFacingDirection * -1);

            rotateMarkersUsed += 2;

            if (currentEntity.canRotate180)
            {
                ShowRotateMarker(2, currentEntity.Vector2Position + currentEntity.facingDirection * -1);
                rotateMarkersUsed++;
            }
        }

        for (int i = rotateMarkers.Length - 1; i >= rotateMarkersUsed; i--)
        {
            rotateMarkers[i].gameObject.SetActive(false);
        }
    }

    public bool IsEntityAtPosition(Vector2 position)
    {
        foreach (Entity e in entities)
        {
            if (e.Vector2Position.Equals(position) && !e.isDead)
                return true;
        }

        return false;
    }

    public bool IsWalkableAtPosition(Vector2 position)
    {
        if (IsEntityAtPosition(position))
            return false;

        foreach (GameObject o in walkables)
        {
            if (o.transform.position.Equals(position))
                return true;
        }

        return false;
    }

    public bool IsPathValid(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;

        for (int i = 0; i < Vector2.Distance(start, end); i++)
        {
            if (IsEntityAtPosition(start + direction * (i + 1)))
                return false;
        }

        return true;
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

        turnOrderImages = new Image[entities.Count];

        for (int i = 0; i < entities.Count; i++)
        {
            turnOrderImages[i] = Instantiate(turnImagePrefab, turnOrderParent).GetComponent<Image>();
            turnOrderImages[i].transform.GetChild(1).GetComponent<Image>().sprite = entities[i].GetComponent<SpriteRenderer>().sprite;
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
        turnOrderImages[currentEntityTurn].color = selectedTurnOrderFrameColor;

        UpdateMoveMarkers();
        UpdateRotateMarkers();

        if (currentAction == 0)
        {
            facingArrow.gameObject.SetActive(true);
            facingArrow.position = currentEntity.Vector2Position;

            if (currentEntity.facingDirection.Equals(Vector2.up))
                facingArrow.rotation = Quaternion.identity;
            else if (currentEntity.facingDirection.Equals(Vector2.right))
                facingArrow.rotation = Quaternion.Euler(0, 0, 270);
            else if (currentEntity.facingDirection.Equals(Vector2.down))
                facingArrow.rotation = Quaternion.Euler(0, 0, 180);
            else if (currentEntity.facingDirection.Equals(Vector2.left))
                facingArrow.rotation = Quaternion.Euler(0, 0, 90);

            if (currentEntity.CompareTag("Player"))
            {
                cameraMovement.trackedObject = currentEntity.transform;
            }

            currentEntity.MovementTurn();
        }
        else
        {
            facingArrow.gameObject.SetActive(false);

            if (adventurerMovedOnTurn)
            {
                NextTurn();
                return;
            }

            currentEntity.AttackTurn();
        }
    }

    private IEnumerator NextTurnDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        currentAction++;

        if (currentHover != null)
            UpdateCursor(currentHover.GetComponent<Object>());

        UpdateMoveMarkers();
        UpdateRotateMarkers();

        turnOrderImages[currentEntityTurn].color = defaultTurnOrderFrameColor;

        if (currentAction > 1)
        {
            currentAction = 0;
            currentEntityTurn++;

            if (currentEntityTurn >= entities.Count)
            {
                currentEntityTurn = 0;
            }

            adventurerMovedOnTurn = false;
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

                        if (currentEntity is Adventurer)
                            adventurerMovedOnTurn = true;
                    }
                }
                else
                {
                    if (o.CompareTag("Walkable"))
                    {
                        if (currentEntity.CanMoveTo(o.Vector2Position))
                        {
                            currentEntity.MoveTo(o.Vector2Position);

                            if (currentEntity is Adventurer)
                                adventurerMovedOnTurn = true;
                        }
                        else
                        {
                            Vector2 rotateTo = (o.Vector2Position - currentEntity.Vector2Position).normalized;

                            if (currentEntity.CanRotateTo(rotateTo))
                            {
                                currentEntity.RotateTo(rotateTo);

                                if (currentEntity is Adventurer)
                                    adventurerMovedOnTurn = true;
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
                        Entity enemy = o.GetComponent<Entity>();
                        currentEntity.Attack(enemy);

                        if (currentEntity is Guardian)
                            enemy.KnockBack(currentEntity.facingDirection);
                    }
                }
            }
        }
    }

    public void PlayerExit()
    {
        SceneManager.LoadScene("ChooseLevel", LoadSceneMode.Single);
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
