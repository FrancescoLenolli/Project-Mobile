using System.Collections.Generic;
using UnityEngine;

public class ShipsView : MonoBehaviour
{
    public enum Cycle { Left, Right }

    private GameManager gameManager = null;
    private List<GameObject> listShips = new List<GameObject>();
    private Vector3 viewPosition = Vector3.zero;
    private int index = 0;
    private int unlockedShipsCount = 0;

    public List<GameObject> listPrefabShips = new List<GameObject>();
    [Space(10)]
    public Transform parentObject = null;

    private void CycleMethod(Cycle cycleType)
    {
        listShips[index].transform.parent = null;
        listShips[index].transform.position = new Vector3(0, 0, -200);

        switch (cycleType)
        {
            case Cycle.Left:
                --index;
                if (index < 0)
                    index = unlockedShipsCount -1;
                break;

            case Cycle.Right:
                ++index;
                if (index == unlockedShipsCount -1)
                    index = 0;
                break;
        }

        listShips[index].transform.SetParent(parentObject);
        listShips[index].transform.position = viewPosition;
    }

    public void InitData()
    {
        gameManager = GameManager.Instance;

        unlockedShipsCount = gameManager.playerData.unlockedShipsCount;
        viewPosition = parentObject.position;

        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(0, 180, 0);

        foreach (GameObject gameObject in listPrefabShips)
        {

            GameObject ship = Instantiate(gameObject, new Vector3(0, 0, -200), newRotation);
            listShips.Add(ship);
        }

        if (unlockedShipsCount > 0)
        {
            listShips[index].transform.SetParent(parentObject);
            listShips[index].transform.position = viewPosition;
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

    public void SetShipsCount()
    {
        ++unlockedShipsCount;
    }

    public void SaveData()
    {
        gameManager.playerData.unlockedShipsCount = unlockedShipsCount;
    }
}
