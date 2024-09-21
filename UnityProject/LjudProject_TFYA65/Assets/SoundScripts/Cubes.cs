using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    public GameObject sampleCube;
    GameObject[] cubes = new GameObject[1024];
    public float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 1024; i++)
        {
            GameObject cube = (GameObject)Instantiate(sampleCube);
            cube.transform.position = transform.position;
            cube.transform.parent = transform;
            cube.name = "sampleCube_" + i;
            transform.eulerAngles = new Vector3(0, -(360f/1024f)*i, 0);
            cube.transform.position = Vector3.forward * 100f;
            cubes[i] = cube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < cubes.Length; i++)
        {
            if(cubes != null)
            {
                cubes[i].transform.localScale = new Vector3(10, SoundControll.samples[i] * maxScale + 2, 10);
            }
        }
    }
}
