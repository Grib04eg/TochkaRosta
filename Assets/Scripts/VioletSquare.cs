using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class VioletSquare : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] float cameraOrthographicSize = 5;
    [SerializeField] ParticleSystem particles;
    System.Action<int> onCollect;

    SpriteRenderer spriteRenderer;
    bool collectable;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(((float)Screen.width / Screen.height * cameraOrthographicSize) / 2f * (index - 1.5f), transform.position.y);
    }

    public void Init(System.Action<int> onCollect = null)
    {
        spriteRenderer.DOKill();
        if (collectableCoroutine != null)
            StopCoroutine(collectableCoroutine);
        if (onCollect != null)
            this.onCollect = onCollect;
        collectable = false;
        particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        spriteRenderer.enabled = true;
        spriteRenderer.color = new Color(0.5903554f, 0.4313725f, 1f);
        spriteRenderer.DOColor(new Color(0.6624473f, 1f, 0.4008439f),5f).onComplete += SetCollectable;
    }

    Coroutine collectableCoroutine;
    void SetCollectable()
    {
        collectable = true;
        spriteRenderer.enabled = false;
        particles.Play();
        collectableCoroutine = StartCoroutine(CollectableTimer(5));
    }

    IEnumerator CollectableTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Init();
    }

    private void OnMouseUpAsButton()
    {
        if (collectable)
        {
            onCollect(100);
            Init();
        }
    }
}
