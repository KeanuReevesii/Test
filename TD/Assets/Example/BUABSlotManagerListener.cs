using ByteDance.Union;
using UnityEngine;
using UnityEngine.UI;

public sealed class BUABSlotManagerListener : IBUSlotABManagerListener
{
    public void onComplete(string slotId, AdSlotType type, int errorCode, string errorMsg)
    {
        UnityDispatcher.PostTask(() =>
        {
            Text SlotIdText = GameObject.Find("Canvas/ScrollView/Viewport/Content/Container/ABSlotPannel/SlotIdText").GetComponent<Text>();
            SlotIdText.text = slotId + "\n" + type;
            Debug.Log("CSJM_Unity "+$"BUABSlotManagerListener onComplete:slotId =  {slotId} , type =  {type.ToString()}, msg: {errorMsg}");
        });

    }
}