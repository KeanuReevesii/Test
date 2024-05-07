using System.Threading;
using ByteDance.Union;
using ByteDance.Union.Mediation;
using UnityEngine;

/**
 * 模板激励视频代码示例
 * 注：模板激励仅支持iOS端穿山甲；iOS融合、安卓穿山甲和融合都统一使用RewardAd
 */
public class ExampleExpressRewardAd
{

    // 加载广告
    public static void LoadExpressRewardAd(Example example)
    {
        // 释放上一次广告
        if (example.expressRewardAd != null)
        {
            example.expressRewardAd.Dispose();
            example.expressRewardAd = null;
        }
        
        // 竖屏
        var codeId = Example.useMediation ? CSJMDAdPositionId.M_REWARD_VIDEO_V_ID : CSJMDAdPositionId.CSJ_REWARD_V_ID;
        // 创造广告参数对象
        var adSlot = new AdSlot.Builder()
            .SetCodeId(codeId) // 必传
            .SetUserID("user123") // 用户id,必传参数
            .SetOrientation(AdOrientation.Vertical) // 必填参数，期望视频的播放方向
            .SetRewardName("银币") // 可选
            .SetRewardAmount(777) // 可选
            .SetMediaExtra("media_extra") // 附加参数，可选
            .Build();
        // 加载广告
        SDK.CreateAdNative().LoadExpressRewardAd(adSlot, new RewardVideoAdListener(example));
    }

    // 展示广告
    public static void ShowExpressRewardAd(Example example)
    {
        if (example.expressRewardAd == null)
        {
            Debug.LogError("CSJM_Unity "+"请先加载广告");
            example.information.text = "请先加载广告";
        }
        else
        {
            // 设置展示阶段的监听器
            example.expressRewardAd.SetRewardAdInteractionListener(new RewardAdInteractionListener(example));
            example.expressRewardAd.SetAgainRewardAdInteractionListener(new RewardAgainAdInteractionListener(example));
            example.expressRewardAd.SetDownloadListener(new AppDownloadListener(example));
            example.expressRewardAd.ShowRewardVideoAd();
        }
    }
    
    /**
     * 广告加载监听器
     */
    public sealed class RewardVideoAdListener : IRewardVideoAdListener
    {
        private Example example;
        public RewardVideoAdListener(Example example)
        {
            this.example = example;
        }
        
        public void OnError(int code, string message)
        {
            Debug.LogError("CSJM_Unity "+$"OnRewardError:{message} on main thread:{Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "OnRewardError: " + message;
            }
        }

        public void OnRewardVideoAdLoad(RewardVideoAd ad)
        {
            Debug.Log("CSJM_Unity "+$"OnRewardVideoAdLoad on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "OnRewardVideoAdLoad";
            }
        }

        public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
        {
            Debug.Log("CSJM_Unity "+$"OnExpressRewardVideoAdLoad on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "OnExpressRewardVideoAdLoad";
            }
            this.example.expressRewardAd = ad;
        }

        public void OnRewardVideoCached()
        {
            Debug.Log("CSJM_Unity "+$"OnRewardVideoCached on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
                this.example.information.text = "OnRewardVideoCached";
        }

        public void OnRewardVideoCached(RewardVideoAd ad)
        {
            Debug.Log("CSJM_Unity "+$"OnRewardVideoCached RewardVideoAd ad on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
        }
    }

    // 广告展示监听器
    public sealed class RewardAdInteractionListener : IRewardAdInteractionListener
    {
        private Example example;

        public RewardAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + $"express rewardVideoAd show on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd show";
            }
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + $"express rewardVideoAd bar click on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd bar click";
            }
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + $"express rewardVideoAd close on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd close";
            }

            if (this.example.expressRewardAd != null)
            {
                this.example.expressRewardAd.Dispose();
                this.example.expressRewardAd = null;
            }
        }

        public void OnVideoSkip()
        {
            Debug.Log("CSJM_Unity " + $"express rewardVideoAd skip on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd skip";
            }
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + $"express rewardVideoAd complete on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd complete";
            }
        }

        public void OnVideoError()
        {
            Debug.LogError("CSJM_Unity " + $"express rewardVideoAd error on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "express rewardVideoAd error";
            }
        }

        public void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo)
        {
            var logString = "OnExpressRewardArrived verify:" + isRewardValid + " rewardType:" + rewardType + " extraInfo: " + extraInfo.ToString() +
                            $" on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}";
            Debug.Log("CSJM_Unity " + logString);
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = logString;
            }
        }
    }

    // 广告再看一个监听器
    public sealed class RewardAgainAdInteractionListener : IRewardAdInteractionListener
    {
        private Example example;

        public RewardAgainAdInteractionListener(Example example)
        {
            this.example = example;
        }

        public void OnAdShow()
        {
            Debug.Log("CSJM_Unity " + $"again express rewardVideoAd show on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            string msg = "Callback --> 第 " + Example.MNowPlayAgainCount + " 次再看 rewardPlayAgain show";
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = msg;
            }
        }

        public void OnAdVideoBarClick()
        {
            Debug.Log("CSJM_Unity " + $"again express rewardVideoAd bar click on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text =
                    "Callback --> 第 " + Example.MNowPlayAgainCount + " 次再看 rewardPlayAgain bar click";
            }
        }

        public void OnAdClose()
        {
            Debug.Log("CSJM_Unity " + "OnAdClose");
        }

        public void OnVideoSkip()
        {
            Debug.Log("CSJM_Unity " + $"again express rewardVideoAd skip on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "Callback --> 第 " + Example.MNowPlayAgainCount + " 次再看 rewardPlayAgain has OnVideoSkip";
            }
        }

        public void OnVideoComplete()
        {
            Debug.Log("CSJM_Unity " + $"again express rewardVideoAd complete on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "Callback --> 第 " + Example.MNowPlayAgainCount + " 次再看 rewardPlayAgain complete";
            }
        }

        public void OnVideoError()
        {
            Debug.LogError("CSJM_Unity " + $"again express rewardVideoAd error on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}");
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = "Callback --> 第 " + Example.MNowPlayAgainCount + " 次再看 rewardPlayAgain error";
            }
        }

        public void OnRewardArrived(bool isRewardValid, int rewardType, IRewardBundleModel extraInfo)
        {
            var logString = "again OnExpressRewardArrived verify:" + isRewardValid + " rewardType:" + rewardType + " extraInfo:" + extraInfo +
                            $" on main thread: {Thread.CurrentThread.ManagedThreadId == Example.MainThreadId}";
            Debug.Log("CSJM_Unity " + logString);
            if (Thread.CurrentThread.ManagedThreadId == Example.MainThreadId)
            {
                this.example.information.text = logString;
            }
        }
    }
}
