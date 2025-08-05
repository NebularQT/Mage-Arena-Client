using System.Collections;
using System.Reflection;
using UnityEngine;

namespace MageArenaClient
{
    public class SpellRepeater : MonoBehaviour
    {
        public void StartShootRoutine(MageBookController controller, GameObject ownerobj, GameObject target, int level, MethodInfo shootMethod)
        {
            StartCoroutine(ShootRoutine(controller, ownerobj, target, level, shootMethod));
        }

        private IEnumerator ShootRoutine(MageBookController controller, GameObject ownerobj, GameObject target, int level, MethodInfo shootMethod)
        {
            Vector3 origin = controller.transform.position;

            switch (ToggleStates.CurrentWardMode)
            {
                case WardMode.Normal:
                    shootMethod.Invoke(controller, new object[] { ownerobj, Camera.main.transform.forward, target, level });
                    break;

                case WardMode.Nova:
                    int missileCount = 20;
                    float angleStep = 360f / missileCount;
                    for (int i = 0; i < missileCount; i++)
                    {
                        float angle = i * angleStep;
                        Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                        shootMethod.Invoke(controller, new object[] { ownerobj, dir, null, level });
                        yield return null;
                    }
                    break;

                case WardMode.Burst:
                    for (int i = 0; i < 2; i++) // shoots 2
                    {
                        Vector3 dir = Camera.main.transform.forward;
                        shootMethod.Invoke(controller, new object[] { ownerobj, dir, target, level });
                        yield return new WaitForSeconds(0.2f); 
                    }
                    break;

                case WardMode.Tornado:
                    int spiralCount = 30;
                    float duration = 1.5f;
                    float spiralRadius = 2f;
                    float heightStep = 0.1f;

                    for (int i = 0; i < spiralCount; i++)
                    {
                        float angle = i * 30f;
                        float radians = angle * Mathf.Deg2Rad;

                        Vector3 offset = new Vector3(Mathf.Cos(radians), i * heightStep, Mathf.Sin(radians)) * spiralRadius;
                        Vector3 dir = offset.normalized;

                        //shoot spell in current tornado x and y
                        shootMethod.Invoke(controller, new object[]
                        {
                        ownerobj,
                        dir,
                        target,
                        level
                        });

                        yield return new WaitForSeconds(duration / spiralCount);
                    }
                    break;

                case WardMode.Rain: //laaaggg

                for (int wave = 0; wave < 10; wave++)
                {
                    for (int i = 0; i <25; i++)
                    {
                        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
                        randomDirection.y = Mathf.Abs(randomDirection.y); //keep it mostly upward

                        shootMethod.Invoke(controller, new object[]
                        {
                        ownerobj,
                        randomDirection,
                        target,
                        level
                        });

                    }

                    yield return new WaitForSeconds(0.3f);
                }
                break;
            }
        }
    }
}