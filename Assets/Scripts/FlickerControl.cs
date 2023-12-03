using System.Collections;
using UnityEngine;

public class FlickerControl : MonoBehaviour
{
    public bool isFlickering = false;
    public int flickerCount = 3; // max number of flickers allowed
    public float timeBetweenFlickers = 0.5f; // time between flickers
    public float flickeringObjectCooldown = 20f; // cooldown period for the flickering object
    private int currentFlickerCount;
    private float lastFlickerTime;

    public GameObject flickeringObject; // Reference to the object you want to flicker

    // Reference to the TorchControl script
    public TorchControl torchControl;

    void Start()
    {
        // Start with the flickering object disabled
        flickeringObject.SetActive(false);

        lastFlickerTime = Time.time; // Initialize the last flicker time

        StartCoroutine(RandomFlicker());
    }

    IEnumerator RandomFlicker()
    {
        while (true)
        {
            // Check if the torch light is enabled in the TorchControl script
            if (torchControl != null && torchControl.torchLight.enabled)
            {
                if (currentFlickerCount < flickerCount)
                {
                    yield return StartCoroutine(FlickeringLight());
                    currentFlickerCount++;

                    // Check if this is the middle flicker
                    if (currentFlickerCount == flickerCount / 2)
                    {
                        StartCoroutine(FlickerRandomObject());
                    }
                }
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

    IEnumerator FlickerRandomObject()
    {
        // Check the cooldown before allowing the flickering object to be triggered again
        if (Time.time - lastFlickerTime > flickeringObjectCooldown)
        {
            // Enable the flickering object
            flickeringObject.SetActive(true);

            // Randomly change the position of the flickering object within specified ranges
            float randomX = Random.Range(-5f, 5f);
            float randomZ = Random.Range(7f, 20.5f);

            // Apply the new position relative to the current position
            flickeringObject.transform.localPosition = new Vector3(randomX, 0f, randomZ);

            // Update the last flicker time
            lastFlickerTime = Time.time;

            // Wait for a short duration (1 second or less)
            yield return new WaitForSeconds(Mathf.Min(0.04f, timeBetweenFlickers));

            // Disable the flickering object
            flickeringObject.SetActive(false);
        }
        else
        {
            Debug.Log("Flickering object on cooldown. Wait for the cooldown period.");
        }
    }
}





//using System.Collections;
//using UnityEngine;

//public class FlickerControl : MonoBehaviour
//{
//    public bool isFlickering = false;
//    public int flickerCount = 3; // max number of flickers allowed
//    public float timeBetweenFlickers = 0.5f; // time between flickers
//    private int currentFlickerCount;


//    void Start()
//    {
//        StartCoroutine(RandomFlicker());
//    }

//    IEnumerator RandomFlicker()
//    {
//        while (true)
//        {
//            if (currentFlickerCount < flickerCount)
//            {
//                yield return StartCoroutine(FlickeringLight());
//                currentFlickerCount++;
//            }

//            yield return new WaitForSeconds(Random.Range(1f, 10f)); // time delay between flickers
//        }
//    }

//    IEnumerator FlickeringLight()
//    {
//        isFlickering = true;

//        int randomFlickerCount = Random.Range(1, 6);

//        for (int i = 0; i < randomFlickerCount; i++)
//        {
//            this.gameObject.GetComponent<Light>().enabled = false;
//            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
//            this.gameObject.GetComponent<Light>().enabled = true;
//            yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
//        }

//        isFlickering = false;

//        yield return new WaitForSeconds(timeBetweenFlickers);
//        currentFlickerCount = 0;
//    }
//}
