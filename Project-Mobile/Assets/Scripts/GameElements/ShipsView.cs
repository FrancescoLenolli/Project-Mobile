using System.Collections.Generic;
using UnityEngine;

public delegate void UnlockedShip();
public class ShipsView : MonoBehaviour
{
    public enum Cycle { Left, Right }

    public event UnlockedShip EventUnlockedShip;

    private GameManager gameManager = null;
    private List<GameObject> listShips = new List<GameObject>();
    private Quaternion newRotation = new Quaternion();
    private Vector3 viewPosition = Vector3.zero;
    private int index = 0;
    private int unlockedShipsCount = 0;

    public List<GameObject> listPrefabShips = new List<GameObject>();
    [Space(10)]
    public Transform parentObject = null;

    private void CycleMethod(Cycle cycleType)
    {
        if (unlockedShipsCount > 0)
        {
            HideShip(index);

            switch (cycleType)
            {
                case Cycle.Left:
                    --index;
                    if (index < 0)
                        index = unlockedShipsCount - 1;
                    break;

                case Cycle.Right:
                    ++index;
                    if (index == unlockedShipsCount)
                        index = 0;
                    break;
            }

            ShowShip(index);
        }
    }

    private void ShowShip(int shipIndex)
    {
        listShips[shipIndex].transform.SetParent(parentObject);
        listShips[shipIndex].transform.position = viewPosition;
        listShips[shipIndex].transform.rotation = newRotation;
    }

    private void HideShip(int shipIndex)
    {
        listShips[shipIndex].transform.parent = null;
        listShips[shipIndex].transform.position = new Vector3(0, 0, -200);
    }

    public void InitData()
    {
        gameManager = GameManager.Instance;

        unlockedShipsCount = gameManager.playerData.unlockedShipsCount;
        index = unlockedShipsCount == 0 ? unlockedShipsCount : unlockedShipsCount - 1;
        viewPosition = parentObject.position;


        newRotation.eulerAngles = new Vector3(0, 180, 0);

        foreach (GameObject gameObject in listPrefabShips)
        {

            GameObject ship = Instantiate(gameObject, new Vector3(0, 0, -200), newRotation);
            listShips.Add(ship);
        }

        if (unlockedShipsCount > 0)
        {
            ShowShip(index);

            if (unlockedShipsCount >= 2)
            {
                CanvasBottom canvasBottom = FindObjectOfType<CanvasBottom>();

                EventUnlockedShip += canvasBottom.ShowCycleButtons;
                EventUnlockedShip?.Invoke();
                EventUnlockedShip -= canvasBottom.ShowCycleButtons;
            }
        }
    }

    public void CycleLeft()
    {
        CycleMethod(Cycle.Left);
    }

    public void CycleRight()
    {
        CycleMethod(Cycle.Right);
    }

    public void ShowNewShip()
    {
        if (unlockedShipsCount <= listPrefabShips.Count)
        {
            HideShip(index);

            ++unlockedShipsCount;

            // Handles first method call.
            index = unlockedShipsCount == 1 ? 0 : unlockedShipsCount - 1;

            ShowShip(index);

            if (unlockedShipsCount == 2)
            {
                CanvasBottom canvasBottom = FindObjectOfType<CanvasBottom>();

                EventUnlockedShip += canvasBottom.ShowCycleButtons;
                EventUnlockedShip?.Invoke();
                EventUnlockedShip -= canvasBottom.ShowCycleButtons;
            }
        }
    }

    public void SaveData()
    {
        gameManager.playerData.unlockedShipsCount = unlockedShipsCount;
    }
}
