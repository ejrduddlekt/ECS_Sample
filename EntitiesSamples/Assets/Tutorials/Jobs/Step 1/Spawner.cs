using UnityEngine;

namespace Tutorials.Jobs.Step1
{
    public class Spawner : MonoBehaviour
    {
        // Ÿ�� ������ �����Ǿ� �����Ƿ� �� �����Ӹ��� Ÿ���� �������� ���
        // �̵��� ��ȯ ������ �� �ʵ忡 ĳ���� ���Դϴ�.
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
                Vector2 dir = Random.insideUnitCircle; //insideUnitCircle�� �������� 1�� ���� �� �ȿ��� �������� ���õ� 2D ���͸� ��ȯ
                seeker.Direction = new Vector3(dir.x, 0, dir.y);
                go.transform.localPosition = new Vector3(
                    Random.Range(0, Bounds.x), 0, Random.Range(0, Bounds.y)); //bounds ������ ������ ��ġ
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