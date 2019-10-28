using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deletemecontroller : MonoBehaviour
{

    public Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        LoginConfigurations.init();
        REST.GetSingleton().SetHeaders(LoginConfigurations.Headers);
        debugText.text = "sending request...";
        REST.GetSingleton().GET("https://apps.flygames.org/volare/healthcheck",(err, resp) => {
            debugText.text = resp;
        });
            
    }


}
