using UnityEngine;
using System.Collections;

public class TurretSystem: MonoBehaviour
{
    public float speed = 3.0f;
    public GameObject m_target = null;
    Vector3 m_lastKnownPosition = Vector3.zero;
    Quaternion m_lookAtRotation;
    public float fireTimer;
    private bool shotready;
    public GameObject bullet;
    public GameObject bulletSpawnpoint;
    // Update is called once per frame
    void Start()
    {
        shotready = true;
    }
    void Update()
    {
        if (m_target)
        {
            if (m_lastKnownPosition != m_target.transform.position)
            {
                m_lastKnownPosition = m_target.transform.position;
                m_lookAtRotation = Quaternion.LookRotation(m_lastKnownPosition - transform.position);
            }

            if (transform.rotation != m_lookAtRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_lookAtRotation, speed * Time.deltaTime);
            }
            if (shotready)
            {
                shoot();

            }
            
        }
    }
    void shoot()
    {
       Transform _bullet =  Instantiate(bullet.transform, transform.position,Quaternion.identity);
        _bullet.transform.rotation = bulletSpawnpoint.transform.rotation;
        shotready = false;
        StartCoroutine(FireRate());
      
    }
    IEnumerator FireRate()
    {
        yield return new WaitForSeconds(fireTimer);
        shotready = true;
    }
    bool SetTarget(GameObject target)
    {
        if (!target)
        {
            return false;
        }

        m_target = target;

        return true;
    }
}
