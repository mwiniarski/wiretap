package wiretap.androidguard;

import android.util.Log;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.List;

public class Logic implements Runnable {
    Serializer s;
    public void run() {
        /*Connection connection = new Connection("192.168.0.199", 8888);
        connection.startClient();
        Thread listeningThread = new Thread(connection, "listen");
        listeningThread.start();

        String sampleMessageString = "Hello there!";
        try {
            byte[] sampleMessage = new byte[256];
            sampleMessage = sampleMessageString.getBytes("US-ASCII");
            connection.sendMessage(sampleMessage);
            //connection.sendMessage(sampleMessage);
            //connection.sendMessage(sampleMessage);
            //connection.sendMessage(sampleMessage);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        } catch (Exception ex) {
          ex.printStackTrace();
        } finally {
            connection.setRun(false);
            connection.closeConnection();
        }//wystawic na zewnatrz metode ktora zamyka socket
        //musze miec joina*/
        /*s = new Serializer();
        try{
            List<byte[]> toSend = s.sendFile("/sdcard/DCIM/Camera/rurushprzykompie.jpg", (byte)1);
            s.sendSplitFile(toSend);
        } catch (IOException e) {
            e.printStackTrace();
            Log.e("Logic", "The serializer could not put the file through!");
        }*/
        Photographer.getNewPhoto();
    }

    public void shutdown() {
        s.senderShutdown();
    }
}
