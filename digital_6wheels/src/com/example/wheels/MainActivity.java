package com.example.wheels;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Timer;
import java.util.TimerTask;

import kankan.wheel.widget.OnWheelChangedListener;
import kankan.wheel.widget.OnWheelScrollListener;
import kankan.wheel.widget.WheelView;
import kankan.wheel.widget.adapters.NumericWheelAdapter;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.MotionEvent;
import android.view.Window;
import android.view.WindowManager;
import android.view.animation.AccelerateInterpolator;
import android.view.animation.AnticipateOvershootInterpolator;
import android.view.animation.BounceInterpolator;
import android.view.animation.Interpolator;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

public class MainActivity extends Activity {
	
	int digitChange[] = {0,0,0,0,0,0,0};
	String countBackup = "0000000";
	Timer aTimer;
	CounterTask aCounterTask;
	View decorView;
	private static final int HIDER_FLAGS = SystemUiHider.FLAG_HIDE_NAVIGATION;
	private SystemUiHider mSystemUiHider;
	private static final boolean AUTO_HIDE = true;
	Button bttn;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		
		/* No Status Bar Snippet   */
		
		requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
		
		// make fullscreen
        makeItFull();        
		
		setContentView(R.layout.activity_main);
		
		initWheel(R.id.number1);
		initWheel(R.id.number2);
		initWheel(R.id.number3);
		initWheel(R.id.number4);
		initWheel(R.id.number5);
		initWheel(R.id.number6);
		
		/* ********************************* */
		
		// make it fullscreen again if it s visible
		
		decorView = getWindow().getDecorView();
		mSystemUiHider = SystemUiHider.getInstance(this, decorView,
				HIDER_FLAGS);
		mSystemUiHider.setup();
		mSystemUiHider
				.setOnVisibilityChangeListener(new SystemUiHider.OnVisibilityChangeListener() {

					@Override
					@TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
					public void onVisibilityChange(boolean visible) {
						if (visible && AUTO_HIDE) {
							// Schedule a hide().
							delayedHide(1000);
						}
					}
				});
		
		/* ********************************* */
		bttn = (Button) findViewById(R.id.button2);
		
		
		bttn.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				mixWheel(R.id.number1, 50, 3500);
				
			}
		});
		
		
		
				
		MyThread thr = new MyThread();
		thr.start();
	}
	
	/* ****************************************** */
	Handler mHideHandler = new Handler();
	Runnable mHideRunnable = new Runnable() {
		@Override
		public void run() {
			mSystemUiHider.hide();
		}
	};
	
	private void delayedHide(int delayMillis) {
		mHideHandler.removeCallbacks(mHideRunnable);
		mHideHandler.postDelayed(mHideRunnable, delayMillis);
	}
	/* ****************************************** */
	
	
	// to make the app fullscreen
	private void makeItFull()
	{
		decorView = getWindow().getDecorView();
		// Hide both the navigation bar and the status bar.
		// SYSTEM_UI_FLAG_FULLSCREEN is only available on Android 4.1 and higher, but as
		// a general rule, you should design your app to hide the status bar whenever you
		// hide the navigation bar.
		int uiOptions = View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
		              | View.SYSTEM_UI_FLAG_FULLSCREEN;
		decorView.setSystemUiVisibility(uiOptions);
		
	}
	
	/* *****  */
	
	/*@Override
	protected void onPostCreate(Bundle savedInstanceState) {
		super.onPostCreate(savedInstanceState);

		// Trigger the initial hide() shortly after the activity has been
		// created, to briefly hint to the user that UI controls
		// are available.
		delayedHide(100);
	}
	
	Handler mHideHandler = new Handler();
	Runnable mHideRunnable = new Runnable() {
		@Override
		public void run() {
			mSystemUiHider.hide();
		}
	};
	
	
	private void delayedHide(int delayMillis) {
		mHideHandler.removeCallbacks(mHideRunnable);
		mHideHandler.postDelayed(mHideRunnable, delayMillis);
	}*/
	
	
	/* *****  */

	
	
	public void onDestroy() 
	{
		if( aTimer != null )
		{
			aTimer.cancel();
			aTimer = null;			
		}
		
		super.onDestroy();
	}
	
	// Wheel scrolled flag
		private boolean wheelScrolled = false;

		// Wheel scrolled listener
		OnWheelScrollListener scrolledListener = new OnWheelScrollListener() {
			@Override
			public void onScrollingStarted(WheelView wheel) {
				wheelScrolled = true;
			}
			@Override
			public void onScrollingFinished(WheelView wheel) {
				wheelScrolled = false;
				
			}
		};

		// Wheel changed listener
		private OnWheelChangedListener changedListener = new OnWheelChangedListener() {
			@Override
			public void onChanged(WheelView wheel, int oldValue, int newValue) {
				if (!wheelScrolled) {
					
				}
			}
		};
		
		
		
		
		
		
		/**
		 * Initializes wheel
		 * @param id the wheel widget Id
		 */
		private void initWheel(int id) {
			WheelView wheel = getWheel(id);
			wheel.setViewAdapter(new NumericWheelAdapter(this, 0, 9));
			wheel.setCurrentItem(0);

			wheel.addChangingListener(changedListener);
			wheel.addScrollingListener(scrolledListener);
			wheel.setCyclic(true);
			wheel.setInterpolator(new AnticipateOvershootInterpolator());
			
		}

		/**
		 * Returns wheel by Id
		 * @param id the wheel Id
		 * @return the wheel with passed Id
		 */
		private WheelView getWheel(int id) {
			return (WheelView) findViewById(id);
		}

		
		private void mixWheel(int id , int tour, int time) {
			WheelView wheel = getWheel(id);
			wheel.scroll(tour, time); // number tours and required time for all
		}
		
		
		private String readStream(InputStream in) 
		{
			 String line = "";
			 StringBuilder result = null;
			  BufferedReader reader = null;
			  try 
			  {
			    reader = new BufferedReader(new InputStreamReader(in));
			    result = new StringBuilder(in.available());
			    while ((line = reader.readLine()) != null) 
			    {
			      result.append(line);
			    }
			  } 
			  
			  catch (IOException e) 
			  {
			    e.printStackTrace();
			  } 
			  finally 
			  {
			    if (reader != null) 
			    {
			      try 
			      {
			        reader.close();				
			      } 
			      catch (IOException e) 
			      {
			        e.printStackTrace();
			      }
			    }
			  }
			  return result.toString();
		}
		
		
		public boolean wifiCheck()
		{
			boolean result = true;		
			
			ConnectivityManager connManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
			
			if( connManager != null )
			{
				NetworkInfo mWifi = connManager.getNetworkInfo(ConnectivityManager.TYPE_WIFI);	
				
				if( mWifi != null )
				{
					if( !mWifi.isConnected() ) result = false;
					if( !mWifi.isAvailable() ) result = false;				
				}
				else if( mWifi == null ) result = false;			
			}
			else if( connManager == null ) result = false;
			
			return result;
		}
		
		class CounterTask extends TimerTask
		{		
			@Override
			public void run() 
			{
				if( wifiCheck() )
				{
				
					String resultedString = "";
					
					
					
					try 
					{
						URL url = new URL("http://countify.co/Push/GetCheckinCount/52f8a295498e04874f8d15ca?pushSecret=MERAGSW5GYALB4L3CLKWUQTQEU1DZM2GD4EZ4J1ARDERYW2O");
						HttpURLConnection con = (HttpURLConnection) url.openConnection();
						resultedString = readStream(con.getInputStream());
					
						
						if ( resultedString.length() > 6 )
						{
							for(int i=1 ; i < 7 ; i++)
								digitChange[i] = modd(Character.getNumericValue(resultedString.charAt(i)) - Character.getNumericValue(countBackup.charAt(i)));
							countBackup = resultedString;
						}	

					} 
					catch (Exception e) 
					{
						e.printStackTrace();
					}
					finally
					{
						runOnUiThread(new Runnable(){
		
						    @Override
						    public void run() {
						    	
						    	int numbers[] = {0,R.id.number1,R.id.number2,R.id.number3,R.id.number4,R.id.number5,R.id.number6};
						    	
						    	for(int i = 1 ; i < 7 ; i++)
						    	{
						    		if(digitChange[i] != 0)
						    		{
						    			mixWheel(numbers[i], digitChange[i] + 50 , 4000);
						    		}
						    		
						    		
						    	}
						    	
						    }});
						
					}			
			
				} // for if statement
			}
			
			
		}
		
		public class MyThread extends Thread
		{
			public void run()
			{	
				
				if( aTimer != null )
				{
					aTimer = null;
				}
					
				aTimer = new Timer();
				aCounterTask = new CounterTask();
					
				aTimer.schedule(aCounterTask, 1000, 1000);			
				
			}
		}
		
		public int modd(int k)
		{
			if(k<0) return k*(-1);
			return k;
		}
		
	
	
}
