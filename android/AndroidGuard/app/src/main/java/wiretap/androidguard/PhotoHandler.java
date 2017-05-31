package wiretap.androidguard;

import java.io.File;
import java.io.FileOutputStream;
import java.text.SimpleDateFormat;
import java.util.Date;

import android.content.Context;
import android.hardware.Camera;
import android.os.Environment;
import android.util.Log;

/**
 * Created by Ebenezer on 2017-05-26.
 */
public class PhotoHandler implements Camera.PictureCallback {
    private static String lastTakenPhotoName = null;
    private final Context context;

    public PhotoHandler(Context context) {
        this.context = context;
    }

    public static String getLastTakenPhotoName() {
        return lastTakenPhotoName;
    }

    @Override
    public void onPictureTaken(byte[] data, Camera camera) {

        File pictureFileDir = getDir();

        if (!pictureFileDir.exists() && !pictureFileDir.mkdirs()) {

            Log.d(MakePhotoActivity.DEBUG_TAG, "Can't create directory to save image.");
            return;

        }

        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyymmddhhmmss");
        String date = dateFormat.format(new Date());
        String photoFile = "Picture_" + date + ".jpg";

        String filename = pictureFileDir.getPath() + File.separator + photoFile;

        File pictureFile = new File(filename);

        Logic logic = new Logic(filename, 1);

        Thread logical = new Thread(logic, "logic");

        try {
            FileOutputStream fos = new FileOutputStream(pictureFile);
            fos.write(data);
            fos.close();
            lastTakenPhotoName = filename;
            logical.start();
            logical.join();
        } catch (Exception error) {
            Log.d(MakePhotoActivity.DEBUG_TAG, "File" + filename + "not saved: "
                    + error.getMessage());
            lastTakenPhotoName = null;
        } finally {
            logic.shutdown();
        }
    }

    private File getDir() {
        File sdDir = Environment
                .getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM);
        return new File(sdDir, "AndroidGuard");
    }
}
