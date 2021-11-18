using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{

    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

        StartCoroutine(NewPos());
    }

    IEnumerator NewPos(){

        while(true){

            Vector3 currentPos = transform.position;
            Vector3 endPos = new Vector3(startPos.x + Random.Range(-4,4), startPos.y, startPos.z + Random.Range(-4,4));

            float t=0;
            while (t<1){
                transform.position = Vector3.Lerp(currentPos, endPos, t);

                t += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
