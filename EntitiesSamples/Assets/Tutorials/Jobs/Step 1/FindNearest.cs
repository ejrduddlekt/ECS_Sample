using UnityEngine;

namespace Tutorials.Jobs.Step1
{
    public class FindNearest : MonoBehaviour
    {
        public void Update()
        {
            // 가장 가까운 타겟을 찾습니다.
            // 거리를 비교할 때 거리의 제곱을 비교하는 것이 더 효율적입니다.
            // 이렇게 함으로써 제곱근 계산을 피할 수 있습니다.
            Vector3 nearestTargetPosition = default; 
            float nearestDistSq = float.MaxValue;
            foreach (var targetTransform in Spawner.TargetTransforms)
            {
                Vector3 offset = targetTransform.localPosition - transform.localPosition;
                float distSq = offset.sqrMagnitude;
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearestTargetPosition = targetTransform.localPosition; 
                }
            }

            Debug.DrawLine(transform.localPosition, nearestTargetPosition);
        }
    }
}
