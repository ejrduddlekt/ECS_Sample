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

            // NativeArray에 BinartSearch(이진탐색)를 사용,
            // BinarySearch는 첫번째 매개변수값을 두번째 매개변수의 방법으로 NativeArray(대상)을 찾으며
            // 일치하는 값이 있으면 Index를 반환하며 없다면 가장 가까운 이전 요소의 인덱스를 비트반전하여 반환한다.
            int startIdx = TargetPositions.BinarySearch(seekerPos, new AxisXComparer { });

            // 여기서 반환값이 음수이면 일치하는 값이 없는 것 입니다.
            // 따라서 startIdx가 음수인 경우 다시 비트를 뒤집지만, 인덱스가 범위 내에 있는지 확인해야 합니다.
            // targetPositions가 배열의 길이보다 크거나 같으면 TargetPositions.Length - 1로 할당
            if (startIdx < 0) startIdx = ~startIdx;
            if (startIdx >= TargetPositions.Length) startIdx = TargetPositions.Length - 1;

            // 가장 가까운 X 좌표를 가진 대상의 위치입니다.
            float3 nearestTargetPos = TargetPositions[startIdx];
            float nearestDistSq = math.distancesq(seekerPos, nearestTargetPos);

            // Search 메서드를 사용하여 배열의 위쪽으로 더 가까운 대상을 검색
            Search(seekerPos, startIdx + 1, TargetPositions.Length, +1, ref nearestTargetPos, ref nearestDistSq);

            // Search 메서드를 사용하여 배열의 아랫쪽으로 더 가까운 대상을 검색
            Search(seekerPos, startIdx - 1, -1, -1, ref nearestTargetPos, ref nearestDistSq);

            //검색이 완료되면 최종값을 저장
            NearestTargetPositions[index] = nearestTargetPos;
        }

        void Search(float3 seekerPos, int startIdx, int endIdx, int step,
                    ref float3 nearestTargetPos, ref float nearestDistSq)
        {
            for (int i = startIdx; i != endIdx; i += step)
            {
                float3 targetPos = TargetPositions[i];
                float xdiff = seekerPos.x - targetPos.x;

                // 탐색기준인 X거리의 제곱이 현재의 최소 거리보다 크다면, 검색을 멈출 수 있습니다. Target은 정렬되어있으므로 굳이 더 검색할 필요가 없다.
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
