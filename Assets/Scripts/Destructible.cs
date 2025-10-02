using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public AudioSource AudioSource = null;
    public AudioClip AudioClip = null;

    public float DestructionDelay = 3f;
    public float ShrinkSpeed = 0.5f;
    public float MinSize = 0.005f;

    private bool _isBroken = false;

    private int _pending = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "DamageInflictor") return;
        if (_isBroken) return;

        _isBroken = true;
        GetComponent<Collider>().enabled = false;
        AudioSource.PlayOneShot(AudioClip);

        for (int i = 0; i < transform.childCount - 1; i++) // -1!
        {
            var part = transform.GetChild(i);
            part.GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(ShrinkPart(part));
            _pending++;
        }

        if (_pending == 0) StartCoroutine(CleanUp());
    }

    private IEnumerator ShrinkPart(Transform part)
    {
        yield return new WaitForSeconds(DestructionDelay);
        while (part != null && part.localScale.x > MinSize)
        {
            part.localScale -= Vector3.one * ShrinkSpeed * Time.deltaTime;
            yield return null;
        }

        if (part != null) Destroy(part.gameObject);

        _pending = Mathf.Max(0, _pending - 1);

        if (_pending == 0) StartCoroutine(CleanUp());
    }

    private IEnumerator CleanUp()
    {
        yield return new WaitForSeconds(DestructionDelay);
        Destroy(gameObject);
    }
}