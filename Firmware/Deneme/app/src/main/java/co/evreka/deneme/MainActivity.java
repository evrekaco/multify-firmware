package co.evreka.deneme;

import android.content.Context;
import android.os.StrictMode;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.squareup.picasso.Picasso;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;


public class MainActivity extends ActionBarActivity {

    TextView nameTv;
    String userName;
    String picUrl;
    JSONObject infoObj;
    Button showButton;
    static InputStream is = null;
    ImageView imageV;
    Context context;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();

        StrictMode.setThreadPolicy(policy);
        context = getApplication().getApplicationContext();
        final String url = "http://46.101.55.98/get_device_data/android1/";

        imageV = (ImageView) findViewById(R.id.imageVi);
        nameTv = (TextView) findViewById(R.id.nameTv);
        showButton = (Button) findViewById(R.id.showButton);
        nameTv.setText("Someone");





        showButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                JSONParser jParser = new JSONParser();
                JSONObject json = jParser.getJSONFromUrl(url);



                try {

                    //infoArray = json.getJSONArray("data");
                    infoObj = (JSONObject)json.get("data");
                } catch (JSONException e) {
                    e.printStackTrace();
                }

                try {
                    userName = infoObj.getString("name");
                    picUrl = infoObj.getString("pic_url");
                } catch (JSONException e) {
                    e.printStackTrace();
                }

                nameTv.setText(userName);
                Picasso.with(context).load(picUrl).into(imageV);
            }
        });


    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
