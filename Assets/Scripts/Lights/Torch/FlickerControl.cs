using System.Collections;
using UnityEngine;

public class FlickerControl : MonoBehaviour
{
    public bool isFlickering = false;
    public int flickerCount = 3; 
    public float timeBetweenFlickers = 0.5f; 
    public float flickeringObjectCooldown = 20f; 
    private int currentFlickerCount;
    private float lastFlickerTime;

    public GameObject flickeringObject; 

    public TorchControl torchControl;

    void Start()
    {
        flickeringObject.SetActive(false);

        lastFlickerTime = Time.time; 

        StartCoroutine(RandomFlicker());
    }

    IEnumerator RandomFlicker()
    {
        while (true)
        {            
            if (torchControl != null && torchControl.torchLight.enabled)
            {
                if (currentFlickerCount < flickerCount)
                {
                    yield return StartCoroutine(FlickeringLight());
                    currentFlickerCount++;

                    if (currentFlickerCount == flickerCount / 2)
                    {
                        StartCoroutine(FlickerRandomObject());
                    }
                }
            }

            yield return new WaitForSeconds(Random.Range(1f, 10f)); 
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

    IEnumerator FlickerRandomObject()
    {        
        if (Time.time - lastFlickerTime > flickeringObjectCooldown)
        {            
            flickeringObject.SetActive(true);

            float randomX = Random.Range(-2f, 2.5f);
            float randomZ = Random.Range(7f, 20.5f);

            flickeringObject.transform.localPosition = new Vector3(randomX, 0f, randomZ);
                   
            lastFlickerTime = Time.time;
                       
            yield return new WaitForSeconds(Mathf.Min(0.04f, timeBetweenFlickers));
                       
            flickeringObject.SetActive(false);
        }
        else
        {
            Debug.Log("Flickering object on cooldown. Wait for the cooldown period.");
        }
    }
}

