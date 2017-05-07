package wiretap.androidguard;

import java.io.UnsupportedEncodingException;

public class Logic implements Runnable {
    public void run() {
        Connection connection = new Connection("37.233.98.52", 8888);
        connection.startClient();
        Thread listeningThread = new Thread(connection, "listen");
        listeningThread.start();

        String sampleMessageString = "Hello there!<EOF>";
        try {
            byte[] sampleMessage = sampleMessageString.getBytes("US-ASCII");
            connection.sendMessage(sampleMessage);
            //connection.sendMessage(sampleMessage);
            //connection.sendMessage(sampleMessage);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
    }
}
