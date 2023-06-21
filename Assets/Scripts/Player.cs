using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float shotCooldown;
    float m_curShotCooldown;
    public GameObject viewFinder;

    bool m_isShoted;
    GameObject m_viewFinderClone;
    private void Awake()
    {
        m_curShotCooldown = shotCooldown;
    }
    private void Start()
    {
        if (viewFinder)
            m_viewFinderClone = Instantiate(viewFinder, Vector3.zero, Quaternion.identity);
    }
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !m_isShoted)
        {
            Shot(mousePos);
        }

        if (m_isShoted)
        {
            m_curShotCooldown -= Time.deltaTime;

            if(m_curShotCooldown <= 0)
            {
                m_isShoted = false;

                m_curShotCooldown = shotCooldown;
            }
            GameGUIManager.Ins.UpdateFireRate(m_curShotCooldown/shotCooldown);
        }

        if (m_viewFinderClone)
            m_viewFinderClone.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
    }
    void Shot(Vector3 mousePos)
    {
        m_isShoted = true;
         
        Vector3 shootDir = Camera.main.transform.position - mousePos;

        shootDir.Normalize();

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootDir);

        if(hits != null && hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.collider && (Vector3.Distance((Vector2)hit.collider.transform.position, (Vector2)mousePos) <= 0.4f)){
                    Bird bird = hit.collider.GetComponent<Bird>();
                    
                    if(bird)
                    {
                        bird.Die();
                    }
                }
            }
        }

        AudioController.Ins.PlaySound(AudioController.Ins.shooting); 
    }  
}
