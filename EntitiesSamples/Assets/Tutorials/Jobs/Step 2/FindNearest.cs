using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Tutorials.Jobs.Step2
{
    public class FindNearest : MonoBehaviour
    {
        // 배열의 크기는 변하지 않기 때문에, 매번 새로운 배열을 생성하는 대신 Awake()에서 배열을 생성하고
        // 이러한 필드에 저장합니다.
        NativeArray<float3> TargetPositions;
        NativeArray<float3> SeekerPositions;
        NativeArray<float3> NearestTargetPositions;


        //배열의 크기 할당
        public void Start()
        {
            Spawner spawner = Object.FindObjectOfType<Spawner>();
            // 이 배열들은 프로그램 실행 동안 존재해야 하므로 Persistent allocator를 사용합니다.
            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }

        //더 이상 필요하지 않을 때 할당 해제를 책임져야 합니다.
        public void OnDestroy()
        {
            TargetPositions.Dispose();
            SeekerPositions.Dispose();
            NearestTargetPositions.Dispose();
        }

        public void Update()
        {
            // 각각의 대상(Transform)을 NativeArray로 복사합니다.
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                // Vector3는 자동으로 float3로 변환됩니다.
                TargetPositions[i] = Spawner.TargetTransforms[i].localPosition;
            }

            // 각각의 Seeker의 Transform을 NativeArray로 복사합니다.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // Vector3는 자동으로 float3로 변환됩니다.
                SeekerPositions[i] = Spawner.SeekerTransforms[i].localPosition;
            }

            // 작업(Job)을 예약하려면 먼저 작업 인스턴스를 생성하고 필드를 채워 넣어야 합니다.
            FindNearestJob findJob = new FindNearestJob
            {
                TargetPositions = TargetPositions,
                SeekerPositions = SeekerPositions,
                NearestTargetPositions = NearestTargetPositions,
            };

            // Schedule() 메서드는 작업 인스턴스(findJob)를 작업 큐에 넣습니다.
            JobHandle findHandle = findJob.Schedule();

            // Complete() 메서드는 핸들(handle)이 나타내는 작업이 실행을 마칠 때까지 반환되지 않습니다.
            // 이로 인해 메인 스레드는 여기서 작업이 완료될 때까지 대기합니다.
            findHandle.Complete();

            // seeker로부터 가장 가까운 대상으로 디버그 라인을 그립니다.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // Vector3는 자동으로 float3로 변환됩니다.
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
            }
        }
    }
}
