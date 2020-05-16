﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;

    private Player _player;
    private Animator _animator ;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("Audio source is null for Enemy");
        }
        if (_player == null)
        {
            Debug.Log("player is null");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("animator is null");
        }
    }


    void Update()
    {
        CalculateMovement();
        if (Time.time >_canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire  = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (uint ii = 0; ii<lasers.Length; ii++)
            {
                lasers[ii].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        // down at _speed m/s
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            // respawn at top with random x pos
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);
        }
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        // to avoid shoot after enemy's death
        _canFire += 1000f;
        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
        }
        else if (other.tag == "Laser")   
        {
             if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.5f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());   
        }
        Destroy(this.gameObject, 2.8f);
    }
}
