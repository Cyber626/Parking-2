using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilCarCreator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CarsListSO carsListSO;
    [SerializeField] private float carCreationTimeMin, carCreationTimeMax;
    private float selectedTime;

    private void Awake()
    {
        selectedTime = Random.Range(carCreationTimeMin / 2, carCreationTimeMax / 2);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, target.position);
        if (selectedTime < 0)
        {
            Vector3 direction = target.position - transform.position;
            if (!Physics2D.Raycast(transform.position, direction, 5))
            {
                GameObject civilCar = Instantiate(carsListSO.carPrefabs[Random.Range(0, carsListSO.carPrefabs.Length)]);
                civilCar.transform.position = transform.position;
                civilCar.transform.Rotate(direction);
                civilCar.GetComponent<CivilCar>().target = target;
            }
            selectedTime = Random.Range(carCreationTimeMin, carCreationTimeMax);
        }

        selectedTime -= Time.deltaTime;

    }
}
