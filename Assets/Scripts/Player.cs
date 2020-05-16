using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public or private reference
    // data type (int, float, bool, string)
    // optional value assigned
    [SerializeField]
    private float _speed = 8.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -0.5f;
    [SerializeField]
    private int _nlives = 3;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedupActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private GameObject _rightEngineVisualizer;
    [SerializeField]
    private GameObject _leftEngineVisualizer;

    [SerializeField]
    private int _score = 0;

    private UIManager _uiManager;

    public AudioSource audioSource;

    void Start()
    {
        // take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Audio source on the player is null");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //if (transform.position.y > 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        //}
        //else if (transform.position.y < -3.8)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);
        //}
        if (transform.position.x < -11)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);
        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_isSpeedupActive == true)
        {
            transform.Translate(direction * 8.5f * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLAser();
        }
    }
    void FireLAser()
    {
        _canFire = Time.time + _fireRate;//
        Vector3 laserPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        if (_isTripleShotActive == true)
        {
            // instantiate the triple shot
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, laserPosition, Quaternion.identity);
        }
        // if tripleShotActive 
        //  instantiate 3 lasers
        // else 1 laser

        // play laser audio clip

//       _audioSource.Play();
    }
    public void Damage()
    {
        // if shields is active
        // deactivate shields
        // return;
        if (_isShieldActive == true) {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _nlives--;
        if (_nlives == 2)
        {
            _rightEngineVisualizer.SetActive(true);
        }
        else if (_nlives == 1)
        {
            _leftEngineVisualizer.SetActive(true);
        }
        _uiManager.UpdateLives(_nlives);
        if (_nlives < 1 )
        {
            // Spawn mgr should stop spawning
            if (_spawnManager != null)
            {
                _spawnManager.OnPlayerDeath();
            }

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        //tripleShotActive becomes true
        // start the power down coroutine for triple shot
        StartCoroutine(TripleShotPowerDown(5));

    }

    public void SpeeupActive()
    {
        _isSpeedupActive = true;
        StartCoroutine(SpeedupPowerDown());
    }
    // IEnumeator TripleShotPowerDownRoutine
    // wait 5 seconds
    // set the triple shot to false
    private IEnumerator TripleShotPowerDown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isTripleShotActive = false;
    }
    private IEnumerator SpeedupPowerDown()
    {
        yield return new WaitForSeconds(5);
        _isSpeedupActive = false;
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    // method to add 10 to the score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    // communicate with ui manager to modify score
}   

