using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DelayExecItem
{
    public float Delay;
    public MonoBehaviour Script;
    public string Method;
}

public class DelayExec : MonoBehaviour
{
    public DelayExecItem[] Items;
    public bool Loop = false;

    IEnumerator WorkCoRo;

    public void StartWork()
    {
        WorkCoRo = Work();
        StartCoroutine(WorkCoRo);
    }

    public void StopWork()
    {
        StopCoroutine(WorkCoRo);
    }

    // Start is called before the first frame update
    IEnumerator Work()
    {
        yield return new WaitForSeconds(0.15f);
        do
        {
            foreach (DelayExecItem Item in Items) {
                yield return new WaitForSeconds(Item.Delay);
                Item.Script?.Invoke(Item.Method, 0.0f);
            }
        } while (Loop);
    }

}
