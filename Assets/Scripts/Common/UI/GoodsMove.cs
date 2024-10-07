using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private Vector3 m_DestPosition;
    private Transform m_Transform;
    private RectTransform m_RectTransform;

    public void SetMove(int idx, Vector3 destPosition)
    {
        m_Transform = transform;
        m_RectTransform = GetComponent<RectTransform>();
        m_DestPosition = new Vector3(destPosition.x, destPosition.y, 0);

        StartCoroutine(MoveCo(idx));
    }

    private IEnumerator MoveCo(int idx)
    {
        yield return new WaitForSeconds(0.1f + 0.08f * idx);

        while (m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, MoveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f);

            yield return null;
        }
        //재화UI위치에 도달했으면 인스턴스를 삭제
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
