using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입의 무기
    [SerializeField] 
    private Hand currentHand;
    
    // 공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;

    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1")) // 마우스 좌클릭
        {
            if (!isAttack)
            {
                // 코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;
        
        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());
        
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                // 충돌 O
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }

            yield return null;
        }
    }

    private bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
