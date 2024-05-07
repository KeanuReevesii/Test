using System.Collections.Generic;
using System.Threading;
using ByteDance.Union;
using UnityEngine;

/**
 * 模板信息流代码示例
 * 注：该接口不支持融合功能，仅支持穿山甲模板代码位
 */
public class ExampleExpressFeedAd
{
    public static void LoadExpressFeedAd(Example example)
    {
        if (example.mExpressFeedad != null)
        {
            example.mExpressFeedad.Dispose();
            example.mExpressFeedad = null;
        }
        var adSlot = new AdSlot.Builder()
            .SetCodeId(CSJMDAdPositionId.CSJ_NATIVE_EXPRESS_ID) // 必传
            ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
            .SetExpressViewAcceptedSize(350, 400)
            .SetAdCount(1) //请求广告数量为1条，只支持同一时间显示1条
            .Build();
        SDK.CreateAdNative().LoadNativeExpressAd(adSlot, new ExpressAdListener(example));
    }

    public static void ShowExpressFeedAd(Example example)
    {
        if (example.mExpressFeedad == null)
        {
            Debug.LogError("CSJM_Unity "+"请先加载广告");
            example.information.text = "请先加载广告";
            return;
        }
        example.mExpressFeedad.SetDislikeCallback(new ExpressAdDislikeCallback(example));
        example.mExpressFeedad.SetDownloadListener(new AppDownloadListener(example));
        example.mExpressFeedad.ShowExpressAd(0, 500);
    }

    // 广告加载监听器
    public sealed class ExpressAdListener : IExpressAdListener
    {
        private Example example;

        public ExpressAdListener(Example example)
        {
            this.example = example;
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "ExpressFeedAdListener";
        }

        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+"onExpressFeedAdError: " + message);
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "onExpressFeedAdError";
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            if (ads != null && ads.Count > 0)
            {
                Debug.Log("CSJM_Unity "+"OnExpressFeedAdLoad, count: " + ads.Count);
                if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                    this.example.information.text = "OnExpressFeedAdLoad";
                this.example.mExpressFeedad = ads[0];
                this.example.mExpressFeedad.SetExpressInteractionListener(new ExpressAdInteractionListener(example));
            }
        }
    }

    // 广告展示监听器
    public sealed class ExpressAdInteractionListener : IExpressAdInteractionListener
    {
        private Example example;

        public ExpressAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdClicked(ExpressAd ad)
        {
            Debug.LogError("CSJM_Unity " + "express feed OnAdClicked");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnAdClicked";
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.LogError("CSJM_Unity " + "express feed OnAdShow");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnAdShow";
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.LogError("CSJM_Unity " + "express feed OnAdViewRenderError");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "express feed OnAdViewRenderError:" + message;
        }

        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
            Debug.LogError("CSJM_Unity " + "express feed OnAdViewRenderSucc");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "express feed OnAdViewRenderSucc";
        }

        public void OnAdClose(ExpressAd ad)
        {
            Debug.LogError("CSJM_Unity " + "express feed OnAdClose");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnAdClose";
        }

        public void onAdRemoved(ExpressAd ad)
        {
            Debug.LogError("CSJM_Unity " + "express feed onAdRemoved");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "onAdRemoved";
        }
    }

    // dislike监听器
    public sealed class ExpressAdDislikeCallback : IDislikeInteractionListener
    {
        private Example example;

        public ExpressAdDislikeCallback(Example example)
        {
            this.example = example;
        }

        public void OnCancel()
        {
            Debug.LogError("CSJM_Unity "+"express feed dislike OnCancel");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "ExpressFeedAdDislikeCallback cancle";
        }

        public void OnShow()
        {
            Debug.LogError("CSJM_Unity "+"express feed dislike OnShow");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "ExpressFeedAdDislikeCallback OnShow";
        }

        public void OnSelected(int var1, string var2, bool enforce)
        {
            Debug.LogError("CSJM_Unity "+"express feed dislike OnSelected:" + var2);
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "ExpressFeedAdDislikeCallback OnSelected";
            //释放广告资源
            if (this.example.mExpressFeedad != null)
            {
                this.example.mExpressFeedad.Dispose();
                this.example.mExpressFeedad = null;
            }
        }
    }
}