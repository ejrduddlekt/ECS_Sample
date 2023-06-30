using UnityEngine;

namespace Tutorials.Jobs.Step1
{
    public class FindNearest : MonoBehaviour
    {
        public void Update()
        {
            // ���� ����� Ÿ���� ã���ϴ�.
            // �Ÿ��� ���� �� �Ÿ��� ������ ���ϴ� ���� �� ȿ�����Դϴ�.
            // �̷��� �����ν� ������ ����� ���� �� �ֽ��ϴ�.
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
