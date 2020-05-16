using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    // speed laser  8
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }
    public void setSpeed(float speed)
    {
        _speed = speed;
    }

    //  translate laser up 
    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {

                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);

        }
    }
    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    public void AssignEnemyLaser()
    {
        this._isEnemyLaser = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if ((other.tag == "Player") && (_isEnemyLaser==true))
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject, 2.8f);
        }
    }
}
