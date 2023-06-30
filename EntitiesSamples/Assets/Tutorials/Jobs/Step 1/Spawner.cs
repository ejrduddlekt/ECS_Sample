using UnityEngine;

namespace Tutorials.Jobs.Step1
{
    public class Spawner : MonoBehaviour
    {
        // 타겟 집합은 고정되어 있으므로 매 프레임마다 타겟을 가져오는 대신
        // 이들의 변환 정보를 이 필드에 캐시할 것입니다.
        public static Transform[] TargetTransforms;

        public GameObject SeekerPrefab;
        public GameObject TargetPrefab;
        public int NumSeekers;
        public int NumTargets;
        public Vector2 Bounds;

        public void Start()
        {
            Random.InitState(123);

            for (int i = 0; i < NumSeekers; i++)
            {
                GameObject go = GameObject.Instantiate(SeekerPrefab);
                Seeker seeker = go.GetComponent<Seeker>();
                Vector2 dir = Random.insideUnitCircle; //insideUnitCircle는 반지름이 1인 단위 원 안에서 무작위로 선택된 2D 벡터를 반환
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y)); //bounds 범위내 무작위 위치
            }

            TargetTransforms = new Transform[NumTargets];
            for (int i = 0; i < NumTargets; i++)
            {
                GameObject go = GameObject.Instantiate(TargetPrefab);
                Target target = go.GetComponent<Target>();
                Vector2 dir = Random.insideUnitCircle;
                target.Direction = new Vector3(dir.x, 0, dir.y);
                TargetTransforms[i] = go.transform;
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y));
            }
        }
    }
}