using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Tutorials.Jobs.Step4
{
    [BurstCompile]
    public struct FindNearestJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;

        public NativeArray<float3> NearestTargetPositions;

        public void Execute(int index)
        {
            float3 seekerPos = SeekerPositions[index];

            // NativeArray�� BinartSearch(����Ž��)�� ���,
            // BinarySearch�� ù��° �Ű��������� �ι�° �Ű������� ������� NativeArray(���)�� ã����
            // ��ġ�ϴ� ���� ������ Index�� ��ȯ�ϸ� ���ٸ� ���� ����� ���� ����� �ε����� ��Ʈ�����Ͽ� ��ȯ�Ѵ�.
            int startIdx = TargetPositions.BinarySearch(seekerPos, new AxisXComparer { });

            // ���⼭ ��ȯ���� �����̸� ��ġ�ϴ� ���� ���� �� �Դϴ�.
            // ���� startIdx�� ������ ��� �ٽ� ��Ʈ�� ��������, �ε����� ���� ���� �ִ��� Ȯ���ؾ� �մϴ�.
            // targetPositions�� �迭�� ���̺��� ũ�ų� ������ TargetPositions.Length - 1�� �Ҵ�
            if (startIdx < 0) startIdx = ~startIdx;
            if (startIdx >= TargetPositions.Length) startIdx = TargetPositions.Length - 1;

            // ���� ����� X ��ǥ�� ���� ����� ��ġ�Դϴ�.
            float3 nearestTargetPos = TargetPositions[startIdx];
            float nearestDistSq = math.distancesq(seekerPos, nearestTargetPos);

            // Search �޼��带 ����Ͽ� �迭�� �������� �� ����� ����� �˻�
            Search(seekerPos, startIdx + 1, TargetPositions.Length, +1, ref nearestTargetPos, ref nearestDistSq);

            // Search �޼��带 ����Ͽ� �迭�� �Ʒ������� �� ����� ����� �˻�
            Search(seekerPos, startIdx - 1, -1, -1, ref nearestTargetPos, ref nearestDistSq);

            //�˻��� �Ϸ�Ǹ� �������� ����
            NearestTargetPositions[index] = nearestTargetPos;
        }

        void Search(float3 seekerPos, int startIdx, int endIdx, int step,
                    ref float3 nearestTargetPos, ref float nearestDistSq)
        {
            for (int i = startIdx; i != endIdx; i += step)
            {
                float3 targetPos = TargetPositions[i];
                float xdiff = seekerPos.x - targetPos.x;

                // Ž�������� X�Ÿ��� ������ ������ �ּ� �Ÿ����� ũ�ٸ�, �˻��� ���� �� �ֽ��ϴ�. Target�� ���ĵǾ������Ƿ� ���� �� �˻��� �ʿ䰡 ����.
                if ((xdiff * xdiff) > nearestDistSq) break;

                float distSq = math.distancesq(targetPos, seekerPos);

                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearestTargetPos = targetPos;
                }
            }
        }
    }

    public struct AxisXComparer : IComparer<float3>
    {
        public int Compare(float3 a, float3 b)
        {
            return a.x.CompareTo(b.x);
        }
    }
}
