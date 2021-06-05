package via.sep4.Model;

import android.content.Context;
import android.content.SharedPreferences;

import via.sep4.MainActivity;

public class AppData {
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is responsible for implementing SharedPreferences and local application settings.
     */

    //TODO: check if encryptedSharedPref would be better
    private static SharedPreferences sharedPref;

    private static AppData appData;

    private AppData() {

    }

    public static void setup(MainActivity context)
    {
        if (appData == null) {
            appData = new AppData();
            sharedPref = context.getPreferences(Context.MODE_PRIVATE);
        }
    }

    public static AppData getInstance()
    {
        return appData;
    }

    public void saveUser(User user) {
        SharedPreferences.Editor editor = sharedPref.edit();
        editor.putString("username", user.getUsername());
        editor.putString("password", user.getPassword());
        editor.apply();
    }

    public String[] retrieveSavedUser() {
        return new String[]{sharedPref.getString("username", ""), sharedPref.getString("password", "")};
    }
}
