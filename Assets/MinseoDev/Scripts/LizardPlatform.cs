using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardPlatform : MonoBehaviour
{
    [SerializeField] private float stumblingTime;
    public Animator animator;
    
    private BoxCollider2D _box;
    private Rigidbody2D _rigid;

    private bool _collapsing = false;
    private float _elapsed = 2f;

    public float lizardSpeed;

    public Rigidbody2D tailRigid;
    
    

    private void Start()
    {
        _box = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_collapsing) _elapsed -= Time.deltaTime;
        if (_box.isTrigger)
        {
            _rigid.gravityScale = 0;
            animator.transform.position += Vector3.right * (animator.GetComponent<SpriteRenderer>().flipX ? lizardSpeed : -lizardSpeed) * Time.deltaTime;
        }
    }

    public void Vanishing()
    {
        if (_collapsing == false)
        {
            _collapsing = true;
            StartCoroutine(Stumbling());
        }
    }

    IEnumerator Stumbling()
    {
        float time = 0;

        int a = 0;
        
        while (time < stumblingTime)
        {
            time += Time.deltaTime;

            yield return null;
        }

        animator.SetTrigger("run");
        _rigid.bodyType = RigidbodyType2D.Dynamic;
        _box.isTrigger = true;
        _rigid.AddTorque(Random.Range(0f, 3f));
        gameObject.layer = 0;
        tailRigid.bodyType = RigidbodyType2D.Dynamic;
        tailRigid.AddForce((Vector2.right + Vector2.up)*3, ForceMode2D.Impulse);
    }
}
