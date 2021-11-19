using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{

    Vector3 startPos;
    public int lo = -4;
    public int hi = 4;
    public float timeBtwn = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        StartCoroutine(NewPos());
    }

    IEnumerator NewPos(){

        while(true){

            Vector3 currentPos = transform.position;
            Vector3 endPos = new Vector3(startPos.x + Random.Range(lo,hi), startPos.y, startPos.z + Random.Range(lo,hi));

            float t=0;
            while (t<1){
                transform.position = Vector3.Lerp(currentPos, endPos, t);

                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(timeBtwn);
        }
    }
}
