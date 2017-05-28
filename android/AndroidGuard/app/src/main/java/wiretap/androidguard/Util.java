package wiretap.androidguard;


import android.content.pm.PackageManager;

/**
 * Created by Ebenezer on 2017-05-25.
 */
public class Util {
    private static String currentAndroidID;
    private static PackageManager packageManager;

    public static String getCurrentAndroidID() {
        return "a" + currentAndroidID;
    }

    public static void setCurrentAndroidID(String currentAndroidID) {
        Util.currentAndroidID = currentAndroidID;
    }

    public static PackageManager getPackageManager() {
        return packageManager;
    }

    public static void setPackageManager(PackageManager packageManager) {
        Util.packageManager = packageManager;
    }


}
