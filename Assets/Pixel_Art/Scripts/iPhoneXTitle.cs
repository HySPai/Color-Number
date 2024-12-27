/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iPhoneXTitle : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if UNITY_IOS
        //fix for iPhone X
        //bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;
        if (Screen.width == 1125)
        {
            //var text = gameObject.GetComponent<Text>();
            //text.fontSize = (int)(text.fontSize * 0.1f);
            // Do something for iPhone X

            var pos = this.transform.localPosition;
            this.transform.localPosition = new Vector3(pos.x, pos.y - 22, pos.z);

            var newScaleRatio = 0.9f;
            var scale = this.transform.localScale;
            this.transform.localScale = new Vector3(scale.x * newScaleRatio, scale.y * newScaleRatio, scale.z);
        }
#endif
	} 
}
