package wiretap.androidguard;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.provider.Settings.Secure;
import android.util.Log;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Util.setCurrentAndroidID(Secure.getString(getApplicationContext().getContentResolver(), Secure.ANDROID_ID));
        Util.setPackageManager(getPackageManager());

        //Log.i("whatever", Util.getCurrentAndroidID());

        /*Logic logic = new Logic();

        Thread logical = new Thread(logic, "logic");
        logical.start();
//tu tez join
        try {
            logical.join();
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            logic.shutdown();
        }*/
        Intent intent = new Intent(this, MakeAudioActivity.class);
        startActivity(intent);
    }
}
