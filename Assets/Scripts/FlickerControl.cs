using System.Collections;
using UnityEngine;

public class FlickerControl : MonoBehaviour
{
    public bool isFlickering = false;
    public int flickerCount = 3; // max number of flickers allowed
    public float timeBetweenFlickers = 0.5f; // time between flickers
    private int currentFlickerCount;


    void Start()
    {
        StartCoroutine(RandomFlicker());
    }

    IEnumerator RandomFlicker()
    {
        while (true)
        {
            if (currentFlickerCount < flickerCount)
            {
                yield return StartCoroutine(FlickeringLight());
                currentFlickerCount++;
            }

            yield return new WaitForSeconds(Random.Range(1f, 10f)); // time delay between flickers
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;

        int randomFlickerCount = Random.Range(1, 6); 

        for (int i = 0; i < randomFlickerCount; i++)
        {
            this.gameObject.GetComponent<Light>().enabled = false;
            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
            this.gameObject.GetComponent<Light>().enabled = true;
            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
        }

        isFlickering = false;

        yield return new WaitForSeconds(timeBetweenFlickers);
        currentFlickerCount = 0; 
    }
}
