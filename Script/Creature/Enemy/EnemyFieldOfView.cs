using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldOfView : MonoBehaviour
{
    [Header("EnemyView")]
    public float viewRadius;
    public Vector3 eyeOffset;
    [Range(0,360)] public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [Space]

    [Header("Debug")]
    public Collider2D targetsInViewRadius;
    [Space]

     public Enemy user;

    public void Start()
    {
        user = GetComponent<Enemy>();
        StartCoroutine(FindTargetWithDelay(0.2f));
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    void FindTargets()
    {
        targetsInViewRadius = Physics2D.OverlapCircle(transform.position + eyeOffset, viewRadius, targetMask);

        if (targetsInViewRadius)
        {
            Transform target = targetsInViewRadius.transform;
            Vector2 dirToTarget = (target.position - transform.position + eyeOffset).normalized;
            if (Vector2.Angle(transform.right * -1 + eyeOffset, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector2.Distance(transform.position + eyeOffset, target.position);

                if (!Physics2D.Raycast(transform.position + eyeOffset, dirToTarget, dstToTarget, obstacleMask))
                {
                    user.Aggro();
                }

            }
            else
                user.IsSearching();
        }
        else
        {
            user.IsSearching();
        }

    }

    public Vector2 DirFormAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y - 90f;
        }

        Vector2 dir = new Vector2(Mathf.Sin((angleInDegrees) * Mathf.Deg2Rad), Mathf.Cos((angleInDegrees) * Mathf.Deg2Rad));

        return dir;
    }
}
