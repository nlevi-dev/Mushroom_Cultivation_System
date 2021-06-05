package via.sep4.Persistence;

import android.content.Context;

import androidx.room.Database;
import androidx.room.Room;
import androidx.room.RoomDatabase;
import androidx.room.TypeConverters;

import via.sep4.Diary.DiaryEntry;
import via.sep4.Model.Hardware;
import via.sep4.Model.SensorData;
import via.sep4.Model.Specimen;
import via.sep4.Model.Status;

@Database(entities = {Specimen.class, SensorData.class, Hardware.class, Status.class, DiaryEntry.class}, version = 2)
@TypeConverters({Converters.class})
public abstract class AppDatabase extends RoomDatabase {
    
    private static AppDatabase instance;
    
    public abstract PersistenceHandler.StatusDAO statusDAO();
    public abstract PersistenceHandler.HardwareDAO hardwareDAO();
    public abstract PersistenceHandler.SpecimenDAO specimenDAO();
    public abstract PersistenceHandler.DiaryEntryDAO diaryEntryDAO();
    
    public static synchronized AppDatabase getInstance(Context context)
    {
        if (instance == null)
        {
            instance = Room.databaseBuilder(context, AppDatabase.class, "entry_database").fallbackToDestructiveMigration().build();
        }
        return instance;
    }
}
