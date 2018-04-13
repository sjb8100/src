package com.hxll.sysinfo;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileFilter;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.util.regex.Pattern;

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Context;
import android.os.Build;
import android.os.Environment;
import android.os.StatFs;
import android.util.DisplayMetrics;
import android.util.Log;

public class SysInfo {

	static String TAG = "SysInfo";
	
	public static String getCpuName(){
		Log.i(TAG,Build.CPU_ABI);
		Log.i(TAG,Build.CPU_ABI2);
		Log.i(TAG,Build.DEVICE);
		Log.i(TAG,Build.MANUFACTURER);
		try {
		      FileReader fr = new FileReader("/proc/cpuinfo");  
		      BufferedReader br = new BufferedReader(fr);  
		      String text = br.readLine();  
		      String[] array = text.split(":\\s+",2);  
		      
		       for(int i = 0; i < array.length; i++){  
		    	   
		    	   Log.i(TAG,array[i]);
		    	   
		       }  
		       return array[1]; 
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}catch (IOException e){  
		     e.printStackTrace();  
		  } 
		
		return null;  
	}
	
	public static int getNumCores() {  
	    //Private Class to display only CPU devices in the directory listing  
	    class CpuFilter implements FileFilter {  
	        @Override  
	        public boolean accept(File pathname) {  
	            //Check if filename is "cpu", followed by a single digit number  
	            if(Pattern.matches("cpu[0-9]", pathname.getName())) {  
	                return true;  
	            }  
	            return false;  
	        }
      
	    }  
	  
	    try {  
	        //Get directory containing CPU info  
	        File dir = new File("/sys/devices/system/cpu/");  
	        //Filter to only list the devices we care about  
	        File[] files = dir.listFiles(new CpuFilter());  
	        Log.d(TAG, "CPU Count: "+files.length);  
	        //Return the number of cores (virtual CPU devices)  
	        return files.length;  
	    } catch(Exception e) {  
	        //Print exception  
	        Log.d(TAG, "CPU Count: Failed.");  
	        e.printStackTrace();  
	        //Default to return 1 core  
	        return 1;  
	    }  
	}
	
		//�鿴SD���ռ�
	public static int getFreeSize()
	{
		long lsize = 0;
		Log.w(TAG, "Get Pone Free Size");
		 if (Environment.getExternalStorageState().equals(android.os.Environment.MEDIA_MOUNTED))
		 {
		     //ȡ��SD���ļ�·��  
		     File path = Environment.getExternalStorageDirectory();   
		     StatFs sf = new StatFs(path.getPath()); 
		    
		     //��ȡ�������ݿ�Ĵ�С(Byte)  
		     @SuppressWarnings("deprecation")
		     long blockSize = sf.getBlockSize();   
		     //���е����ݿ������  
		     @SuppressWarnings("deprecation")
		     long freeBlocks = sf.getAvailableBlocks();  
		     //����SD�����д�С  
		     //return freeBlocks * blockSize;  //��λByte  
		     //return (freeBlocks * blockSize)/1024;   //��λKB  
		     lsize = ((freeBlocks * blockSize)/1024 /1024); //��λMB  			 
		 }
		 //��ȡ�����ڴ�·��
	      File path=Environment.getDataDirectory();
	      StatFs statFs=new StatFs(path.getPath());
	      @SuppressWarnings("deprecation")
	      long blockSizer=statFs.getBlockSize();
	      @SuppressWarnings("deprecation")
	      long availableBlocksr=statFs.getAvailableBlocks();
	      //long blockSize=statFs.getBlockSizeLong();
	      //long availableBlocks=statFs.getAvailableBlocksLong();
	     long lsizer = ((blockSizer*availableBlocksr)/1024/1024);	
	     if(lsize < lsizer)
	     {
	    	 lsize = lsizer;
	     }
	     return (int)lsize;
	}
	
	public static String getMinCpuFreq() {    
        String result = "";    
        ProcessBuilder cmd;    
        try {    
                String[] args = { "/system/bin/cat",    
                                "/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq" };    
                cmd = new ProcessBuilder(args);    
                Process process = cmd.start();    
                InputStream in = process.getInputStream();    
                byte[] re = new byte[24];    
                while (in.read(re) != -1) {    
                        result = result + new String(re);    
                }    
                in.close();    
        } catch (IOException ex) {    
                ex.printStackTrace();    
                result = "N/A";    
        }    
        return result.trim();    
	}
	
	public static long getRamMemory(){  
		//Activity activity = UnityPlayer.currentActivity;
		//Context context = activity.getApplicationContext();
		
        String str1 = "/proc/meminfo";// ϵͳ�ڴ���Ϣ�ļ�  
        String str2;  
        String[] arrayOfString;  
        long initial_memory = 0;   
  
        try  
        {  
            FileReader localFileReader = new FileReader(str1);  
            BufferedReader localBufferedReader = new BufferedReader(  
            localFileReader, 8192);  
            str2 = localBufferedReader.readLine();// ��ȡmeminfo��һ�У�ϵͳ���ڴ��С  
  
            arrayOfString = str2.split("\\s+");  
            for (String num : arrayOfString) {  
                Log.i(str2, num + "\t");  
            }  
  
            initial_memory = Integer.valueOf(arrayOfString[1]).intValue() * 1024;// ���ϵͳ���ڴ棬��λ��KB������1024ת��ΪByte  
            localBufferedReader.close();  
  
        } catch (IOException e) {  
        }  
        //return Formatter.formatFileSize(context, initial_memory);// Byteת��ΪKB����MB���ڴ��С���  
        System.out.println("���˴�--->>>"+initial_memory/(1024*1024));  
        return initial_memory/(1024*1024);  
    }  
	
	@SuppressWarnings("deprecation")
	public static long getTotalInternalMemorySize() {      
        File path = Environment.getDataDirectory();      
        StatFs stat = new StatFs(path.getPath());      
        @SuppressWarnings("deprecation")
		long blockSize = stat.getBlockSize();      
        long totalBlocks = stat.getBlockCount();      
        return totalBlocks * blockSize;      
    } 
	
	public static String getScreenResolution(){  
	    DisplayMetrics dm = new DisplayMetrics();  
	    Activity activity = UnityPlayer.currentActivity;
	    activity.getWindowManager().getDefaultDisplay().getMetrics(dm);  
	   String strOpt = dm.widthPixels + " * " + dm.heightPixels;  
	   return strOpt;  
	}
}
