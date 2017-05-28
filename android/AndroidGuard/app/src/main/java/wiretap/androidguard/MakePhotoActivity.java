package wiretap.androidguard;

import android.graphics.SurfaceTexture;
import android.support.v7.app.AppCompatActivity;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.hardware.Camera;
import android.hardware.Camera.CameraInfo;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import java.io.IOException;

public class MakePhotoActivity extends AppCompatActivity {
    public final static String DEBUG_TAG = "MakePhotoActivity";
    private Camera camera;
    private int cameraId = 0;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_make_photo);

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
                            new PhotoHandler(getApplicationContext()));
                } catch (IOException e) {
                    e.printStackTrace();
                    camera.stopPreview();
                } finally {
                    super.finish();
                }
            }
        }
    }

    /*public void onClick(View view) {
        camera.startPreview();
        camera.takePicture(null, null,
                new PhotoHandler(getApplicationContext()));
    }*/

    private int findFrontFacingCamera() {
        int cameraId = -1;
        // Search for the front facing camera
        int numberOfCameras = Camera.getNumberOfCameras();
        for (int i = 0; i < numberOfCameras; i++) {
            CameraInfo info = new CameraInfo();
            Camera.getCameraInfo(i, info);
            if (info.facing == CameraInfo.CAMERA_FACING_FRONT) {
                Log.d(DEBUG_TAG, "Camera found");
                cameraId = i;
                break;
            }
        }
        return cameraId;
    }

    @Override
    protected void onPause() {
        if (camera != null) {
            camera.release();
            camera = null;
        }
        super.onPause();
    }
}
