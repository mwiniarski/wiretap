package wiretap.androidguard;

import android.util.Log;

import java.io.*;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by Ebenezer on 2017-05-14.
 */
public class Serializer {
    private final int packetSize = 256;
    private Sender sender;

    public List<byte[]> sendFile(String path, byte dataType) throws IOException {
        List<byte[]> splitFile = new LinkedList<byte[]>();
        byte[] buffer = new byte[packetSize];
        int bytesRead = 0;
        int framesCount = 0;
        int lastFrameSize= 0;
        InputStream stream = new FileInputStream(path);
        while((bytesRead = stream.read(buffer, 0, buffer.length)) > 0) {
            if(bytesRead == packetSize) {
                splitFile.add(buffer.clone());
            } else {
                lastFrameSize = bytesRead;
                splitFile.add(buffer.clone());
            }
            framesCount++;
        }
        byte byteCount = (byte) (framesCount % packetSize);
        byte byteCount2 = (byte) (framesCount / packetSize);
        Log.i("Serializer", "Frame count: " + framesCount);
        int fileSize = byteCount + (byteCount2 * 256);
        Log.i("Serializer", "file size: " + fileSize);

        byte[] firstFrame = {dataType, byteCount2, byteCount, (byte)lastFrameSize};
        splitFile.add(0, firstFrame);

        stream.close();
        return splitFile;
    }

    public List<byte[]> test() {
        List<byte[]> send = new LinkedList<byte[]>();
        byte[] frame = {1, 2, 3, 4};
        send.add(frame);
        return send;
    }

    public List<byte[]> splitFile() {
        List<byte[]> toSend = new LinkedList<byte[]>();
        byte[] partOne = {1, 0, 1, 3};
        byte[] partTwo = {120, 100, 100};

        toSend.add(partOne);
        toSend.add(partTwo);

        return toSend;
    }

    public boolean sendSplitFile(List<byte[]> splitFile) {
        sender = new Sender("192.168.0.199", 8888);
        boolean started = sender.startClient();
        if(!started) {
            Log.e("Serializer", "Could not establish connection");
            return false;
        }
        boolean sent = true;
        int counter = 0;
        for (byte[] part:splitFile) {
            sent = sender.sendFrame(part);
            counter++;
            if(!sent) break;
        }
        if(sent) {
            Log.i("Serializer", "File sent!");
            return true;
        }
        Log.e("Serializer", "Failed to send a file!");
        Log.i("Serializer", "Packets sent: " + counter);
        return false;
    }

    public void senderShutdown() {
        if(sender != null) {
            sender.closeConnection();
        }
    }
}
