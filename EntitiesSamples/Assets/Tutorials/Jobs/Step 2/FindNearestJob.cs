using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// �츮�� Unity.Mathematics.float3 ��ſ� Vector3�� ����� ���̸�,
// Unity.Mathematics.math.distancesq ��ſ� Vector3.sqrMagnitude�� ����� ���Դϴ�.
using Unity.Mathematics;

namespace Tutorials.Jobs.Step2
{
    // �۾��� Burst �������Ϸ��� BurstCompile �Ӽ��� �����մϴ�.
    [BurstCompile]
    public struct FindNearestJob : IJob
    {
        // �۾��� �׼����� ��� �����ʹ� �ش� �۾��� �ʵ忡 ���ԵǾ�� �մϴ�.
        // �� ��쿡�� �۾��� float3�� �� ���� �迭�� �ʿ�� �մϴ�.

        // �۾������� �д� �迭 �� �÷��� �ʵ�� ReadOnly �Ӽ����� ǥ���ؾ� �մϴ�.
        // �� ��쿡�� ������ �ʿ������� ������, �����͸� ReadOnly�� ǥ���ϸ� �۾� �����ٷ���
        // ���� �����ϰ� �۾��� ���ÿ� ������ �� ���� ���� �ֽ��ϴ�.
        // (�ڼ��� ������ "Intro to jobs" �� �����ϼ���.)

        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;


        // SeekerPositions[i]���� NearestTargetPositions[i]�� ���� ����� ��� ��ġ�� �Ҵ��� ���Դϴ�.
        public NativeArray<float3> NearestTargetPositions;


        // 'Execute'�� IJob �������̽��� ������ �޼����Դϴ�.
        // �۾� �����尡 �۾��� ������ �� �� �޼��尡 ȣ��˴ϴ�.
        public void Execute()
        {
            // �� ��Ŀ���� ��� �������� ���� �Ÿ��� ����մϴ�.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                float3 seekerPos = SeekerPositions[i];
                float nearestDistSq = float.MaxValue; //float.MaxValue: ǥ���� �� �ִ� �ִ� ��
                for (int j = 0; j < TargetPositions.Length; j++)
                {
                    float3 targetPos = TargetPositions[j];
                    float distSq = math.distancesq(seekerPos, targetPos); //Seeker�� target ���

                    //�ּҰ� ����
                    if (distSq < nearestDistSq)
                    {
                        nearestDistSq = distSq;
                        NearestTargetPositions[i] = targetPos;
                    }
                }
            }
        }
    }
}