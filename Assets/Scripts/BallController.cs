using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float impulseForce = 3f;
    private bool ignoreNextCollision;

    private Vector3 startPosition;

    public int perfectPass;

    public float superSpeed = 8;

    private bool isSuperSpeedActive;
    public int perfectPassCount = 1;
    public AudioSource collisionAudio;

    public GameObject splash;

    private bool isCollidingWithSide;

    void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Goalontroller>())
        {
            perfectPass++;
        }

        if (ignoreNextCollision)
        {
            return;
        }

        if (isSuperSpeedActive && !collision.transform.GetComponent<Goalontroller>())
        {
            Destroy(collision.transform.parent.gameObject, 0.2f);
        }
        else
        {
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
            if (deathPart)
            {
                GameManager.singleton.RestartLevel();
                return;
            }

            // Verificar colisión en la parte superior del objeto
            if (collision.contacts[0].normal == Vector3.up)
            {
                AddSplash(collision);
                collisionAudio.Play();
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

                ignoreNextCollision = true;
                Invoke("AllowNextCollision", 0.2f);
            }
            else if (collision.contacts[0].normal != Vector3.up && collision.contacts[0].normal != Vector3.down)
            {
                // Colisión con los bordes laterales
                isCollidingWithSide = true;
            }
        }

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void OnCollisionExit(Collision collision)
{
    if (collision.contactCount > 0)
    {
        if (collision.contacts[0].normal != Vector3.up && collision.contacts[0].normal != Vector3.down)
        {
            // Salida de colisión con los bordes laterales
            isCollidingWithSide = false;
        }
    }
}


    private void FixedUpdate()
    {
        if (perfectPass >= perfectPassCount && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;

            rb.AddForce(Vector3.down * superSpeed, ForceMode.Impulse);
        }

        if (!isCollidingWithSide)
        {
            // Aplicar una fuerza constante hacia abajo para que la bola continúe bajando sin frenarse
            rb.AddForce(Vector3.down * impulseForce * 0.1f, ForceMode.Force);
        }
    }

    private void AllowNextCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPosition;
    }

    public void AddSplash(Collision collision)
    {
        GameObject newSplash;
        newSplash = Instantiate(splash);
        newSplash.transform.SetParent(collision.transform);
        newSplash.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.11f, this.transform.position.z);
        Destroy(newSplash, 3);
    }
}
