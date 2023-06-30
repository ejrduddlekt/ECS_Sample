using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

// 우리는 Unity.Mathematics.float3 대신에 Vector3를 사용할 것이며,
// Unity.Mathematics.math.distancesq 대신에 Vector3.sqrMagnitude를 사용할 것입니다.
using Unity.Mathematics;

namespace Tutorials.Jobs.Step2
{
    // 작업을 Burst 컴파일하려면 BurstCompile 속성을 포함합니다.
    [BurstCompile]
    public struct FindNearestJob : IJob
    {
        // 작업이 액세스할 모든 데이터는 해당 작업의 필드에 포함되어야 합니다.
        // 이 경우에는 작업이 float3의 세 개의 배열을 필요로 합니다.

        // 작업에서만 읽는 배열 및 컬렉션 필드는 ReadOnly 속성으로 표시해야 합니다.
        // 이 경우에는 엄격히 필요하지는 않지만, 데이터를 ReadOnly로 표시하면 작업 스케줄러가
        // 보다 안전하게 작업을 동시에 실행할 수 있을 수도 있습니다.
        // (자세한 내용은 "Intro to jobs" 를 참조하세요.)

        [ReadOnly] public NativeArray<float3> TargetPositions;
        [ReadOnly] public NativeArray<float3> SeekerPositions;


        // SeekerPositions[i]에는 NearestTargetPositions[i]에 가장 가까운 대상 위치를 할당할 것입니다.
        public NativeArray<float3> NearestTargetPositions;


        // 'Execute'는 IJob 인터페이스의 유일한 메서드입니다.
        // 작업 스레드가 작업을 실행할 때 이 메서드가 호출됩니다.
        public void Execute()
        {
            // 각 시커에서 모든 대상까지의 제곱 거리를 계산합니다.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                float3 seekerPos = SeekerPositions[i];
                float nearestDistSq = float.MaxValue; //float.MaxValue: 표현할 수 있는 최대 값
                for (int j = 0; j < TargetPositions.Length; j++)
                {
                    float3 targetPos = TargetPositions[j];
                    float distSq = math.distancesq(seekerPos, targetPos); //Seeker와 target 계산

                    //최소값 갱신
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