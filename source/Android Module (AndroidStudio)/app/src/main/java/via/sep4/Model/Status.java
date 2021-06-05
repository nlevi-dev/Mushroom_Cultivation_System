package via.sep4.Model;

import androidx.room.Entity;
import androidx.room.ForeignKey;
import androidx.room.PrimaryKey;

import java.io.Serializable;

@Entity(foreignKeys = {@ForeignKey(entity = Specimen.class,
        parentColumns = "specimen_key",
        childColumns = "specimen_key"
)})
public class Status implements Serializable
{

    @PrimaryKey
    private int entry_key;

    private long entry_time; // long due to Unix time
    private String stage_name;
    private int specimen_key;

    public int getEntry_key() {
        return entry_key;
    }

    public void setEntry_key(int entry_key) {
        this.entry_key = entry_key;
    }

    public long getEntry_time() {
        return entry_time;
    }

    public void setEntry_time(long entry_time) {
        this.entry_time = entry_time;
    }

    public String getStage_name() {
        return stage_name;
    }

    public void setStage_name(String stage_name) {
        this.stage_name = stage_name;
    }

    public int getSpecimen_key() {
        return specimen_key;
    }

    public void setSpecimen_key(int specimen_key) {
        this.specimen_key = specimen_key;
    }
}
