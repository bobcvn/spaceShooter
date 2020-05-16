using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate object on z axis.
        transform.Rotate(UnityEngine.Vector3.forward * _rotateSpeed*Time.deltaTime);
    }
    // check for laser collision (trigger)
    // instantiate explosion at asteroids position
    // destroy explosion after 3 s
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, UnityEngine.Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
