using UnityEngine;

public class CarSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private CameraRig cameraRig;

    private int currentIndex = 0;

    private void Start()
    {
        for (int i = 0; i < cars.Length; i++)
            cars[i].SetActive(i == 0);

        if (cars.Length > 0)
            cameraRig.target = cars[0].transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchTo(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchTo(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchTo(2);
    }

    private void SwitchTo(int index)
    {
        if (index >= cars.Length || index == currentIndex) return;

        Vector3 position = cars[currentIndex].transform.position;
        Quaternion rotation = cars[currentIndex].transform.rotation;

        cars[currentIndex].SetActive(false);

        cars[index].transform.position = position;
        cars[index].transform.rotation = rotation;
        cars[index].SetActive(true);

        foreach (Transform t in cars[index].GetComponentsInChildren<Transform>(true))
            t.gameObject.SetActive(true);

        currentIndex = index;
        cameraRig.target = cars[index].transform;
    }
}
