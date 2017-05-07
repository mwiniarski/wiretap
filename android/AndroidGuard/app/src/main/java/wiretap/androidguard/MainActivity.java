package wiretap.androidguard;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Logic logic = new Logic();

        Thread logical = new Thread(logic, "logic");
        logical.start();

    }
}
