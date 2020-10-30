using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    IEnumerator Start()
    {
        Advertisement.Initialize("3884039", true);

        while (!Advertisement.IsReady())
        {
            yield return null;
        }

        Advertisement.Show(); // Neat.
    }
}
