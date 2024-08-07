using System.Collections;
using UnityEngine;

public class FroggerController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Vector3 _spawnPosition;
    private float _farthesRow;
    private AudioSource _playerAudio;

    public Sprite idleSprite;
    public Sprite leapSprite;
    public Sprite deadSprite;
    public AudioClip jumpSound;
    public AudioClip deadthSound;




    
    // Start is called before the first frame update
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnPosition = transform.position;
        _playerAudio = GetComponent<AudioSource>();
    }




    // Update is called once per frame
    void Update()
    {
        // MOVIMIENTO DE LA RANA 
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Move(Vector3.up);
            _playerAudio.PlayOneShot(jumpSound, 1f);
        }
        else  if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            Move(Vector3.down);
            _playerAudio.PlayOneShot(jumpSound, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            Move(Vector3.left);
            _playerAudio.PlayOneShot(jumpSound, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            Move(Vector3.right);
            _playerAudio.PlayOneShot(jumpSound, 1f);
        }
    }


    void Move(Vector3 direction)
    {
        Vector3 destination = transform.position + direction;
        
        //para detectar sobre que layer estoy para actuar
        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));
        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));
        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null)
        {
            return;
        }

        if (platform != null)
        {
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        if (obstacle != null && platform == null)
        {
            transform.position = destination;
            Death();
        }
        else
        {
            if (destination.y > _farthesRow)
            {
                _farthesRow = destination.y;
                FindObjectOfType<GameManager>().AdvancedRow();
            }

            StartCoroutine(Leap(destination));
        }

    }

    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;
        
        float elapsed = 0f;
        float duration = 0.125f;

        _spriteRenderer.sprite = leapSprite;

        while(elapsed < duration)
        {
            // le puso t en el tutorial, es tiempo? volver a ver 56:50 min
            float tiempo = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, destination, tiempo);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
        _spriteRenderer.sprite = idleSprite;
    }

    public void Death()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        _spriteRenderer.sprite = deadSprite;
        enabled = false;
        _playerAudio.PlayOneShot(deadthSound, 1f);
        FindObjectOfType<GameManager>().Died();
    }

    public void Respawn()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        transform.position = _spawnPosition;
        _farthesRow = _spawnPosition.y;
        _spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && transform.parent == null)
        {
            Death();
        }
    }


}
