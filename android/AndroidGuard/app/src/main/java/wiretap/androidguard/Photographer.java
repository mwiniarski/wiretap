package wiretap.androidguard;

import android.app.Activity;
import android.content.Context;
import android.content.pm.PackageManager;
import android.graphics.ImageFormat;
import android.graphics.SurfaceTexture;
import android.hardware.Camera;
import android.os.Environment;
import android.util.Log;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by Ebenezer on 2017-05-25.
 */
public class Photographer {
    private static class PhotoHandler implements Camera.PictureCallback {

        //private final Context context;
        private String lastTakenPhotoName = null;

        /*public PhotoHandler(Context context) {
            this.context = context;
        }*/

        @Override
        public void onPictureTaken(byte[] data, Camera camera) {

            Log.d("Debug", "dupa8");
            File pictureFileDir = getDir();

            if (!pictureFileDir.exists() && !pictureFileDir.mkdirs()) {

                Log.wtf("Photographer", "Can't create directory to save image.");
                return;

            }

            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyymmddhhmmss");
            String date = dateFormat.format(new Date());
            String photoFile = "Picture_" + date + ".jpg";

            String filename = pictureFileDir.getPath() + File.separator + photoFile;

            File pictureFile = new File(filename);

            try {
                FileOutputStream fos = new FileOutputStream(pictureFile);
                fos.write(data);
                fos.close();
                lastTakenPhotoName = photoFile;
                Log.i("Photographer", "New Image saved:" + photoFile);
            } catch (Exception error) {
                Log.wtf("Photographer", "File" + filename + "not saved: " + error.getMessage());
                lastTakenPhotoName = null;
            }
        }

        private File getDir() {
            File sdDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
            return new File(sdDir, "AndroidGuard");
        }

        public String getLastTakenPhotoName() {
            return lastTakenPhotoName;
        }
    }

    private static class PhotoTaker{
        private final static String DEBUG_TAG = "PhotoTaker";
        private Camera camera;
        private int cameraId = 0;
        private PhotoHandler handler;

        public PhotoTaker(PhotoHandler h) {
            if(!Util.getPackageManager().hasSystemFeature(PackageManager.FEATURE_CAMERA)) {
                Log.e("Photographer", "The device has no camera!");
            } else {
                cameraId = findFrontFacingCamera();
                if(cameraId < 0) {
                    Log.e("Photographer", "No front facing camera found.");
                } else {
                    camera = Camera.open(cameraId);
                    if(camera == null)
                        Log.d("Debug", "dupa7");
                    try {
                        Log.d("Debug", "dupa1");
                        camera.setPreviewTexture(new SurfaceTexture(10));
                        Log.d("Debug", "dupa2");
                    } catch (IOException e) {
                        e.printStackTrace();
                        Log.d("Debug", "dupa3");
                    }
                }
            }
            handler = h;
        }

        public void shoot() {
            Log.d("Debug", "dupa4");
            if(camera != null) {
                Camera.Parameters params = camera.getParameters();
                params.setPreviewSize(640, 480);
                params.setFlashMode(Camera.Parameters.FLASH_MODE_OFF);
                params.setPictureFormat(ImageFormat.JPEG);
                camera.setParameters(params);
                camera.startPreview();
                camera.takePicture(null, null, handler);
                Log.d("Debug", "dupa5");
            } else {
                Log.e("Photographer", "No camera opened!");
            }
        }

        private int findFrontFacingCamera() {
            int cameraId = -1;

            int numberOfCameras = Camera.getNumberOfCameras();
            for (int i = 0; i < numberOfCameras; i++) {
                Camera.CameraInfo info = new Camera.CameraInfo();
                Camera.getCameraInfo(i, info);
                if(info.facing == Camera.CameraInfo.CAMERA_FACING_FRONT) {
                    Log.i("Photographer", "Camera found.");
                    cameraId = i;
                    break;
                }
            }
            return cameraId;
        }

        public void releaseCamera() {
            if(camera != null) {
                camera.release();
                camera = null;
            }
        }
    }

    public static String getNewPhoto(/*Context c*/) {
        PhotoHandler photoHandler = new PhotoHandler(/*c*/);
        PhotoTaker photoTaker = new PhotoTaker(photoHandler);
        photoTaker.shoot();
        String photoName = photoHandler.getLastTakenPhotoName();
        photoTaker.releaseCamera();
        return photoName;
    }
}
