package wiretap.androidguard;

//import android.os.AsyncTask;
import android.util.Log;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.*;

/**
 * Created by Ebenezer on 2017-05-14.
 */
/*public class Sender {
    private InetSocketAddress socAddr;
    private Socket soc;
    private final int reconnectTimeout = 1;
    private String response;
    private final int sendTimeout = 5;
    private final int connectionTimeout = 5;
    private final int ackTimeout = 5;
    private int sendData;
    private Boolean ack;
    private Boolean connected;

    private class ConnectTask extends AsyncTask<Void, Void, Void> {
        protected Void doInBackground(Void... v) {
            try {
                Log.i("Sender", "Connecting...");
                soc.connect(socAddr);
                connected = true;
            } catch(IOException ioe) {
                ioe.printStackTrace();
            } catch(NullPointerException npe) {
                npe.printStackTrace();
            } catch(Exception e) {
                e.printStackTrace();
            }
            return null;
        }
    }

    private class GetMessageTask extends AsyncTask<Void, Void, Void> {
        protected Void doInBackground(Void... v) {
            byte[] bytes = new byte[2];
            DataInputStream dataInputStream = null;
            try {
                int bytesReceived = 0;

                dataInputStream = new DataInputStream(soc.getInputStream());
                //dataInputStream.readFully(bytes, 0, bytes.length);
                bytesReceived=dataInputStream.read(bytes);
                if(bytesReceived==1) {
                    byte ack_ = bytes[0];
                    ack = ack_ == 1;
                }
            } catch (IOException e) {
                e.printStackTrace();
            }
            return null;
        }
    }

    private class SendFrameTask extends AsyncTask<byte[], Void, Boolean> {
        protected Boolean doInBackground(byte[]... mes) {
            DataOutputStream dataOutputStream = null;
            try {
                dataOutputStream = new DataOutputStream(soc.getOutputStream());
                dataOutputStream.write(mes[0]);
                //dataOutputStream.close();
                return true;
            } catch (IOException e) {
                e.printStackTrace();
                return false;
            }
        }
    }


}*/
/*public class StateObject {
    public AsynchronousSocketChannel channel;
}

public class Sender {

}*/

public class Sender /*implements Runnable*/{
    private Socket soc;
    private int reconnectTimeout = 1;
    private int connectTimeout = 5;
    private int sendTimeout = 5;
    private int ackTimeout = 5;
    private InetAddress inetAddress;
    private int port;
    private InetSocketAddress socketAddress;
    private boolean connected = false;
    private boolean ack = false;
    private int sendData = 0;
    private int bufferSize = 1;

    public Sender(String ipAddress, int _port) {
        try {
            inetAddress = InetAddress.getByName(ipAddress);
            port = _port;
            socketAddress = new InetSocketAddress(inetAddress, port);
        }
        catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    public boolean startClient() {
        soc = new Socket();
        try {
            establishConnection();
            return connected;
        } catch(Exception e) {
            Log.e("Sender", "An exception occurred while establishing connection. Sender could not be started.");
            return false;
        }

    }

    public boolean sendFrame(byte[] message) {
        ack = false;
        sendData = 0;
        DataOutputStream dataOutputStream = null;
        try {
            dataOutputStream = new DataOutputStream(soc.getOutputStream());
            dataOutputStream.write(message);
            getMessage();
            if(ack) {
                Log.i("Sender", "ACK sent back!");
            } else {
                Log.i("Sender", "ACK not sent back - file not sent...");
            }
            //dataOutputStream.close();
            return ack;
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
    }

    public void getMessage() throws IOException{
        ack = false;
        byte[] buffer = new byte[bufferSize];
        DataInputStream dataInputStream = null;
        int bytesRead = 0;

        dataInputStream = new DataInputStream(soc.getInputStream());
        //dataInputStream.readFully(bytes, 0, bytes.length);
        bytesRead = dataInputStream.read(buffer);
        if(bytesRead == 1) {
            byte ak = buffer[0];
            ack = ak == 1;
        }
        //dataInputStream.close();
    }

    public void establishConnection() throws IOException {
        Log.i("CON", "Connecting...");
        soc.connect(socketAddress, connectTimeout * 1000);
        Log.i("CON", "Socket connected to" + soc.getRemoteSocketAddress());
        soc.setSoTimeout(ackTimeout * 1000);
        connected = true;
    }

    public void closeConnection() {
        try {
            Log.i("Sender", "Socket connection closing...");
            soc.shutdownInput();
            soc.shutdownOutput();
            soc.close();
            connected = false;
            ack = false;
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}