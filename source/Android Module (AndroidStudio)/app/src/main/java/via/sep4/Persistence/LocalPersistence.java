package via.sep4.Persistence;

import android.content.Context;

import androidx.room.Room;

public class LocalPersistence { //implements Room Library, provides database access
    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is responsible for implementing the Room library, enabling connections to the local SQLite database.
     */

    private static AppDatabase database;

    private LocalPersistence() {

    }

    public static AppDatabase getDatabaseInstance(Context context) {
        if (database == null) {
            database = Room.databaseBuilder(context, AppDatabase.class, "MushroomDatabase").build();
        }
        return database;
    }
}
