package wiretap.androidguard;

import android.app.Service;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.SurfaceTexture;
import android.hardware.Camera;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

import java.io.IOException;

public class MakePhotoService extends Service {

    public final static String DEBUG_TAG = "MakePhotoService";
    private Camera camera;
    private int cameraId = 0;

    public MakePhotoService() {
    }

    @Override
    public void onCreate() {
        super.onCreate();
        //setContentView(R.layout.activity_make_photo);

        // do we have a camera?
        if (!getPackageManager()
                .hasSystemFeature(PackageManager.FEATURE_CAMERA)) {
            Toast.makeText(this, "No camera on this device", Toast.LENGTH_LONG)
                    .show();
        } else {
            cameraId = findFrontFacingCamera();
            if (cameraId < 0) {
                Toast.makeText(this, "No front facing camera found.",
                        Toast.LENGTH_LONG).show();
            } else {
                camera = Camera.open(cameraId);

                try {
                    Camera.Parameters parameters = camera.getParameters();
                    parameters.setPictureSize(1024, 768);
                    camera.setParameters(parameters);
                    camera.setPreviewTexture(new SurfaceTexture(10));
                    camera.startPreview();
                    camera.takePicture(null, null,
                            new PhotoHandler(getApplicationContext(), this));
                } catch (IOException e) {
                    e.printStackTrace();
                    camera.stopPreview();
                } finally {
                    //camera.release();
                    //super.finish();
                }
            }
        }
    }

    private int findFrontFacingCamera() {
        int cameraId = -1;
        // Search for the front facing camera
        int numberOfCameras = Camera.getNumberOfCameras();
        for (int i = 0; i < numberOfCameras; i++) {
            Camera.CameraInfo info = new Camera.CameraInfo();
            Camera.getCameraInfo(i, info);
            if (info.facing == Camera.CameraInfo.CAMERA_FACING_FRONT) {
                Log.d(DEBUG_TAG, "Camera found");
                cameraId = i;
                break;
            }
        }
        return cameraId;
    }

    @Override
    public void onDestroy() {
        if (camera != null) {
            camera.release();
            camera = null;
        }
        super.onDestroy();
        Log.i("MakePhotoService", "Destroying...");
    }

    @Override
    public IBinder onBind(Intent intent) {
        // TODO: Return the communication channel to the service.
        //throw new UnsupportedOperationException("Not yet implemented");
        return null;
    }
}
