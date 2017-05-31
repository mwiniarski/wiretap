package wiretap.androidguard;

import android.util.Log;

import java.io.IOException;
import java.util.List;

public class Logic implements Runnable {
    Serializer s;
    String filePath;
    int dataType;
    public Logic(String filePath, int dataType) {
        this.filePath = filePath;
        this.dataType = dataType;
    }
    public void run() {

        s = new Serializer();
        try{
            List<byte[]> toSend = s.sendFile(filePath, (byte)dataType);
            s.sendSplitFile(toSend);
        } catch (IOException e) {
            e.printStackTrace();
            Log.e("Logic", "The serializer could not put the file through!");
        }

    }

    public void shutdown() {
        s.senderShutdown();
    }
}
