/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件由会员免费分享，如果商用，请务必联系原著购买授权！

daily assets update for try.

U should buy a license from author if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class NotificationTest : MonoBehaviour
{
    void Awake()
    {
        LocalNotification.ClearNotifications();
    }

    public void OneTime()
    {
        LocalNotification.SendNotification(1, 5000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }

    public void OneTimeBigIcon()
    {
        LocalNotification.SendNotification(1, 5000, "Title", "Long message text with big icon", new Color32(0xff, 0x44, 0x44, 255), true, true, true, "app_icon");
    }

    public void OneTimeWithActions()
    {
        LocalNotification.Action action1 = new LocalNotification.Action("background", "In Background", this);
        action1.Foreground = false;
        LocalNotification.Action action2 = new LocalNotification.Action("foreground", "In Foreground", this);
        LocalNotification.SendNotification(1, 5000, "Title", "Long message text with actions", new Color32(0xff, 0x44, 0x44, 255), true, true, true, null, "boing", "default", action1, action2);
    }

    public void Repeating()
    {
        LocalNotification.SendRepeatingNotification(1, 5000, 60000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
    }

    public void Stop()
    {
        LocalNotification.CancelNotification(1);
    }

    public void OnAction(string identifier)
    {
        Debug.Log("Got action " + identifier);
    }
}
