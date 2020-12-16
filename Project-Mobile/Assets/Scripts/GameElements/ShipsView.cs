using System.Collections.Generic;
using UnityEngine;

public class ShipsView : MonoBehaviour
{
    public enum Cycle { Left, Right }

    private List<GameObject> listShips = new List<GameObject>();
    private Vector3 viewPosition = Vector3.zero;
    private int index = 0;

    public List<GameObject> listPrefabShips = new List<GameObject>();
    [Space(10)]
    public Transform parentObject = null;

    private void Start()
    {
        viewPosition = parentObject.position;

        foreach (GameObject gameObject in listPrefabShips)
        {
            GameObject ship = Instantiate(gameObject, new Vector3(0, 0, -200), new Quaternion(0, 0, 0, 0));
            listShips.Add(ship);
        }

        listShips[index].transform.SetParent(parentObject);
        listShips[index].transform.localPosition = viewPosition;
    }

    public void CycleLeft()
    {
        CycleMethod(Cycle.Left);
    }

    public void CycleRight()
    {
        CycleMethod(Cycle.Right);
    }

    private void CycleMethod(Cycle cycleType)
    {
        listShips[index].transform.parent = null;
        listShips[index].transform.position = new Vector3(0, 0, -200);

        switch (cycleType)
        {
            case Cycle.Left:
                --index;
                if (index < 0)
                    index = listPrefabShips.Count - 1;
                break;

            case Cycle.Right:
                ++index;
                if (index == listPrefabShips.Count)
                    index = 0;
                break;
        }

        listShips[index].transform.SetParent(parentObject);
        listShips[index].transform.localPosition = viewPosition;
    }
}
