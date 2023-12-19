using System.Collections;
using UnityEngine;

public class KepkaBullet : PatternBullet
{
    public Transform model;

    public Animator animator;
    public AudioSource audioSource;

    public AudioClip awakeSound;
    public AudioClip launchSound;

    public float Speed = 1.0f;
    public bool Reverse = false;

    private Vector2 moveDir = Vector2.zero;

    private bool look = false;
    private bool move = false;

    public void Initialize(bool isRight = false)
    {
        if (isRight)
            model.localScale = new Vector2(-model.localScale.x, model.localScale.y);

        StartCoroutine(animCoroutine());
    }

    private void FixedUpdate()
    {
        if (look)
            moveDir = (BattleManager.instance.player.transform.position - transform.position).normalized;

        if (move)
            transform.Translate(moveDir * Speed * Time.fixedDeltaTime);
    }

    private IEnumerator animCoroutine()
    {
        animator.SetTrigger("START");

        audioSource.clip = awakeSound;
        audioSource.Play();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));

        look = true;

        yield return new WaitForSeconds(1f);

        look = false;

        yield return new WaitForSeconds(0.5f);

        audioSource.clip = launchSound;
        audioSource.Play();

        move = true;

        yield return new WaitForSeconds(2f);
    
        move = false;

        Destroy(gameObject);
    }
}