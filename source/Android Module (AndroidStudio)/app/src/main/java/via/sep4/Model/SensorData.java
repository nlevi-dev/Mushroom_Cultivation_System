package via.sep4.Model;

import androidx.room.Entity;
import androidx.room.ForeignKey;
import androidx.room.PrimaryKey;

@Entity(foreignKeys = {@ForeignKey(entity = Specimen.class,
        parentColumns = "specimen_key",
        childColumns = "specimenKey"
)})
public class SensorData {

    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class stores all saved sensor data at a given timestamp.
     */

    @PrimaryKey
    private int key;

    private long entry_time; //long needed due to unix time (stored in 64-bit format)
    private float air_temperature;
    private float air_humidity;
    private float air_co2;
    private float light_level;
    private float desired_air_temperature;
    private float desired_air_humidity;
    private float desired_air_co2;
    private float desired_light_level;
    private int specimenKey;

    public int getKey() {
        return key;
    }

    public void setKey(int key) {
        this.key = key;
    }

    public long getEntry_time() {
        return entry_time;
    }

    public void setEntry_time(long entry_time) {
        this.entry_time = entry_time;
    }

    public float getAir_temperature() {
        return air_temperature;
    }

    public void setAir_temperature(float air_temperature) {
        this.air_temperature = air_temperature;
    }

    public float getAir_humidity() {
        return air_humidity;
    }

    public void setAir_humidity(float air_humidity) {
        this.air_humidity = air_humidity;
    }

    public float getAir_co2() {
        return air_co2;
    }

    public void setAir_co2(float air_co2) {
        this.air_co2 = air_co2;
    }

    public float getLight_level() {
        return light_level;
    }

    public void setLight_level(float light_level) {
        this.light_level = light_level;
    }

    public float getDesired_air_temperature() {
        return desired_air_temperature;
    }

    public void setDesired_air_temperature(float desired_air_temperature) {
        this.desired_air_temperature = desired_air_temperature;
    }

    public float getDesired_air_humidity() {
        return desired_air_humidity;
    }

    public void setDesired_air_humidity(float desired_air_humidity) {
        this.desired_air_humidity = desired_air_humidity;
    }

    public float getDesired_air_co2() {
        return desired_air_co2;
    }

    public void setDesired_air_co2(float desired_air_co2) {
        this.desired_air_co2 = desired_air_co2;
    }

    public float getDesired_light_level() {
        return desired_light_level;
    }

    public void setDesired_light_level(float desired_light_level) {
        this.desired_light_level = desired_light_level;
    }

    public int getSpecimenKey() {
        return specimenKey;
    }

    public void setSpecimenKey(int specimenKey) {
        this.specimenKey = specimenKey;
    }
}
