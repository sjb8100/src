package com.hxll.gvoice;



import android.app.Activity;
import android.content.Context;
import android.util.Log;

import com.talkingsdk.MainApplication;
import com.talkingsdk.SdkBase;
import com.tencent.gcloud.voice.GCloudVoiceEngine;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;


public class Gvoice {

	static Context context = null;
	static Activity activity = null;
	static int ret = -1;
	
	public static int InitGVoiceEngine(){
		
		activity = UnityPlayer.currentActivity;
		Log.i("Gvoice", "InitGVoiceEngineWithoutSDK");
		
		if(activity == null){
			Log.i("Gvoice", "activity == null:");
			return 1;
		}
		
		context = activity.getApplicationContext();
		if(context == null){
			Log.i("Gvoice", "context == null :");
			return 2;
		}
		
		activity.runOnUiThread(new Runnable() {
			
			@Override
			public void run() {
				// TODO Auto-generated method stub
				ret = GCloudVoiceEngine.getInstance().init(context, activity);
				Log.i("Gvoice", "init :" + ret);
			}
		});
		Log.i("Gvoice", "init :" + ret);
		return ret; 
	}
	
	
//	public static int InitGVoiceEngine(){
//		
//		SdkBase sdkbase = MainApplication.getInstance().getSdkInstance();
//		if(sdkbase == null){
//			Log.i("Gvoice", "sdkbase== null:");
//			return 3;
//		}
//		
//		activity = sdkbase.getCurrentContext();
//		if(activity == null){
//			Log.i("Gvoice", "activity == null :");
//			return 2;
//		}
//		
//		context = activity.getApplicationContext();
//		if(context == null){
//			Log.i("Gvoice", "context == null:");
//			return 1;
//		}
//			
//		Log.i("Gvoice", "GCloudVoiceEngine init ");
//		activity.runOnUiThread(new Runnable() {
//			
//			@Override
//			public void run() {
//				// TODO Auto-generated method stub
//				ret = GCloudVoiceEngine.getInstance().init(context, activity);
//				Log.i("Gvoice", "init :" + ret);
//			}
//		});
//		
//		
//		
//		return ret; 
//	}
}
