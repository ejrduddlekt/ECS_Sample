using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Tutorials.Jobs.Step2
{
    public class FindNearest : MonoBehaviour
    {
        // �迭�� ũ��� ������ �ʱ� ������, �Ź� ���ο� �迭�� �����ϴ� ��� Awake()���� �迭�� �����ϰ�
        // �̷��� �ʵ忡 �����մϴ�.
        NativeArray<float3> TargetPositions;
        NativeArray<float3> SeekerPositions;
        NativeArray<float3> NearestTargetPositions;


        //�迭�� ũ�� �Ҵ�
        public void Start()
        {
            Spawner spawner = Object.FindObjectOfType<Spawner>();
            // �� �迭���� ���α׷� ���� ���� �����ؾ� �ϹǷ� Persistent allocator�� ����մϴ�.
            TargetPositions = new NativeArray<float3>(spawner.NumTargets, Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.NumSeekers, Allocator.Persistent);
        }

        //�� �̻� �ʿ����� ���� �� �Ҵ� ������ å������ �մϴ�.
        public void OnDestroy()
        {
            TargetPositions.Dispose();
            SeekerPositions.Dispose();
            NearestTargetPositions.Dispose();
        }

        public void Update()
        {
            // ������ ���(Transform)�� NativeArray�� �����մϴ�.
            for (int i = 0; i < TargetPositions.Length; i++)
            {
                // Vector3�� �ڵ����� float3�� ��ȯ�˴ϴ�.
                TargetPositions[i] = Spawner.TargetTransforms[i].localPosition;
            }

            // ������ Seeker�� Transform�� NativeArray�� �����մϴ�.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // Vector3�� �ڵ����� float3�� ��ȯ�˴ϴ�.
                SeekerPositions[i] = Spawner.SeekerTransforms[i].localPosition;
            }

            // �۾�(Job)�� �����Ϸ��� ���� �۾� �ν��Ͻ��� �����ϰ� �ʵ带 ä�� �־�� �մϴ�.
            FindNearestJob findJob = new FindNearestJob
            {
                TargetPositions = TargetPositions,
                SeekerPositions = SeekerPositions,
                NearestTargetPositions = NearestTargetPositions,
            };

            // Schedule() �޼���� �۾� �ν��Ͻ�(findJob)�� �۾� ť�� �ֽ��ϴ�.
            JobHandle findHandle = findJob.Schedule();

            // Complete() �޼���� �ڵ�(handle)�� ��Ÿ���� �۾��� ������ ��ĥ ������ ��ȯ���� �ʽ��ϴ�.
            // �̷� ���� ���� ������� ���⼭ �۾��� �Ϸ�� ������ ����մϴ�.
            findHandle.Complete();

            // seeker�κ��� ���� ����� ������� ����� ������ �׸��ϴ�.
            for (int i = 0; i < SeekerPositions.Length; i++)
            {
                // Vector3�� �ڵ����� float3�� ��ȯ�˴ϴ�.
                Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
            }
        }
    }
}
