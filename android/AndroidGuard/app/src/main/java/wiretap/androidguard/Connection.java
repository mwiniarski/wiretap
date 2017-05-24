package wiretap.androidguard;

import android.util.Log;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Created by H.Jatkowski on 2017-05-07.
 */
public class Connection /*implements Runnable*/{
    /*private Socket soc;
    private int reconnectTimeout = 1;
    private InetAddress inetAddress;
    private int port;
    private Boolean run = true;

    public Connection(String ipAddress, int _port) {
        try {
            inetAddress = InetAddress.getByName(ipAddress);
            port = _port;
        }
        catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    public Connection() {
        this("192.168.0.199", 8888);
    }

    public void startClient() {
        soc = new Socket();

        establishConnection();
    }

    public void establishConnection() {
        try {
            Log.i("CON", "Connecting...");
            soc.connect(new InetSocketAddress(inetAddress, port));
            Log.i("CON", "Socket connected to" + soc.getRemoteSocketAddress());
        } catch (IOException e) {
            e.printStackTrace();
            handleDisconnection();
        }
    }

    public void handleDisconnection() {
        Log.i("CON", "Connection with server lost. Reconnecting in " + reconnectTimeout + " minutes...");
        try {
            Thread.sleep(reconnectTimeout * 60 * 1000);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        startClient();
    }

    public boolean sendMessage(byte[] message) {
        DataOutputStream dataOutputStream = null;
        try {
            dataOutputStream = new DataOutputStream(soc.getOutputStream());
            dataOutputStream.write(message);
            //dataOutputStream.close();
            return true;
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
    }

    public byte[] getMessage(int length) {
        byte[] bytes = new byte[length];
        DataInputStream dataInputStream = null;
        try {
            int bytesReceived = 0;

            dataInputStream = new DataInputStream(soc.getInputStream());
            //dataInputStream.readFully(bytes, 0, bytes.length);
            while(bytesReceived!=length) {
                bytesReceived+=dataInputStream.read(bytes);
            }
            //dataInputStream.close();
            return bytes;
        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    public void closeConnection() {
        try {
            soc.shutdownInput();
            soc.shutdownOutput();
            soc.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void listen() {
        while(run) {
            byte[] message = getMessage(256);
            if(message != null)
                handleMessage(message);
            else {
                Log.e("MSG", "Could not receive message.");
                handleDisconnection();
            }
        }
    }

    private void handleMessage(byte[] message) {
        try {
            Log.i("MSG", "Echoed test = " + new String(message, "US-ASCII"));
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
    }

    public void run() {
        listen();
    }

    public void setRun(Boolean value) {
        run = value;
    }*/
}
