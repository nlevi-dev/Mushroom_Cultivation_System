package sep4.iot.gateway.model;
import java.io.Serializable;


/**
 * SensorEntry object is the container for the data received from Loriot as an uplink
 * and data to be sent to Loriot as a downlink
 *
 * @see java.io.Serializable
 * @author Mihai Anghelus
 * @author Daria Popa
 * @version 1.0
 * @since 26-05-2021
 */
public class SensorEntry implements Serializable {
    private int entry_key;
    private int user_key;
    private String hardware_id;
    private long entry_time;

    private float air_temperature;
    private float air_humidity;
    private int air_co2;
    private float light_level;

    private float desired_air_temperature;
    private float desired_air_humidity;
    private int desired_air_co2;
    private float desired_light_level;

    /**
     * Default constructor
     */
    public SensorEntry(){}

    /**
     * Constructor used when getting Sensor entries from the client in order to construct
     * a new downlink message
     *
     * @param entry_key - unique entry key of the sensor entry
     * @param hardware_id - unique hex identification string for the hardware
     * @param user_key - unique user identification number
     * @param entry_time - the entry time of the sensor entry
     * @param air_temperature - measured air temperature
     * @param air_humidity - measured air humidity
     * @param air_co2 - measured air co2 value
     * @param light_level - measured light level
     * @param desired_air_temperature - desired value for the air temperature
     * @param desired_air_humidity - desired value for the air humidity
     * @param desired_air_co2 - desired value for the co2 level
     * @param desired_light_level - desired value for the light level
     */
    public SensorEntry(int entry_key, String hardware_id, int user_key, long entry_time, float air_temperature, float air_humidity,
                       int air_co2, float light_level, float desired_air_temperature, float desired_air_humidity,
                       int desired_air_co2, float desired_light_level) {
        this.entry_key = entry_key;
        this.user_key = user_key;
        this.hardware_id = hardware_id;
        this.entry_time = entry_time;
        this.air_temperature = air_temperature;
        this.air_humidity = air_humidity;
        this.air_co2 = air_co2;
        this.light_level = light_level;
        this.desired_air_temperature = desired_air_temperature;
        this.desired_air_humidity = desired_air_humidity;
        this.desired_air_co2 = desired_air_co2;
        this.desired_light_level = desired_light_level;
    }

    /**
     * Constructor used to save new sensor entries from the hardware and prepare them to be
     * sent to the client
     *
     * @param entry_key - unique entry key of the sensor entry
     * @param hardware_id - unique hex identification string for the hardware
     * @param user_key - unique user identification number
     * @param entry_time - the entry time of the sensor entry
     * @param air_temperature - measured air temperature
     * @param air_humidity - measured air humidity
     * @param air_co2 - measured air co2 value
     * @param light_level - measured light level
     */
    public SensorEntry(int entry_key, int user_key, String hardware_id, long entry_time, float air_temperature,
                       float air_humidity, int air_co2, float light_level) {
        this.entry_key = entry_key;
        this.user_key = user_key;
        this.hardware_id = hardware_id;
        this.entry_time = entry_time;
        this.air_temperature = air_temperature;
        this.air_humidity = air_humidity;
        this.air_co2 = air_co2;
        this.light_level = light_level;

        //dummy values for desired fields to indicate received data
        desired_air_co2=0;
        desired_air_humidity=0;
        desired_light_level=0;
        desired_air_temperature=0;
    }

    /**
     * Getter for the hardware EUI
     * @return a reference to the String value of the hardware eui
     */
    public String getHardware_id() {
        return hardware_id;
    }

    /**
     * Setter for the hardware EUI
     * @param hardware_id - the new EUI value
     */
    public void setHardware_id(String hardware_id) {
        this.hardware_id = hardware_id;
    }

    /**
     * Getter for the desired air temperature
     * @return a float value representing the desired air temperature
     */
    public float getDesired_air_temperature() {
        return desired_air_temperature;
    }

    /**
     * Setter for the desired air temperature
     * @param desired_air_temperature - the new value for the desired air temperature
     */
    public void setDesired_air_temperature(float desired_air_temperature) {
        this.desired_air_temperature = desired_air_temperature;
    }

    /**
     * Getter for the desired air humidity
     * @return a float value representing the desired air humidity
     */
    public float getDesired_air_humidity() {
        return desired_air_humidity;
    }

    /**
     * Setter for the desired air humidity
     * @param desired_air_humidity - the new desired air humidity value
     */
    public void setDesired_air_humidity(float desired_air_humidity) {
        this.desired_air_humidity = desired_air_humidity;
    }

    /**
     * Getter for the desired co2 value
     * @return an integer value representing the desired co2 value
     */
    public int getDesired_air_co2() {
        return desired_air_co2;
    }

    /**
     * Setter for the desired co2 value
     * @param desired_air_co2 - the new value for the desired co2 value
     */
    public void setDesired_air_co2(int desired_air_co2) {
        this.desired_air_co2 = desired_air_co2;
    }

    /**
     * Getter for the desired light level
     * @return a float value representing the desired light level
     */
    public float getDesired_light_level() {
        return desired_light_level;
    }

    /**
     * Setter for the desired light value
     * @param desired_light_level - the new value for the desired light value
     */
    public void setDesired_light_level(float desired_light_level) {
        this.desired_light_level = desired_light_level;
    }

    /**
     * Getter for the entry key
     * @return an integer value representing the entry key of the sensor entry
     */
    public int getEntry_key() {
        return entry_key;
    }

    /**
     * Setter for the entry key
     * @param entry_key - the new value for the entry key of the sensor entry
     */
    public void setEntry_key(int entry_key) {
        this.entry_key = entry_key;
    }

    /**
     * Getter for the user key
     * @return an integer value representing the user key
     */
    public int getUser_key() {
        return user_key;
    }

    /**
     * Setter for the user key
     * @param user_key - the new value for the user key
     */
    public void setUser_key(int user_key) {
        this.user_key = user_key;
    }

    /**
     * Getter for the entry time
     * @return a long value representing the entry time of the sensor entry
     */
    public long getEntry_time() {
        return entry_time;
    }

    /**
     * Setter for the entry time
     * @param entry_time - the new value for the entry time of the sensor entry
     */
    public void setEntry_time(long entry_time) {
        this.entry_time = entry_time;
    }

    /**
     * Getter for the air temperature
     * @return a float value representing the air temperature
     */
    public float getAir_temperature() {
        return air_temperature;
    }

    /**
     * Setter for the air temperature value
     * @param air_temperature - the new value for the air temperature
     */
    public void setAir_temperature(float air_temperature) {
        this.air_temperature = air_temperature;
    }

    /**
     * Getter for the air humidity
     * @return a float value representing the air humidity
     */
    public float getAir_humidity() {
        return air_humidity;
    }

    /**
     * Setter for the air humidity value
     * @param air_humidity - the new value for the air humidity
     */
    public void setAir_humidity(float air_humidity) {
        this.air_humidity = air_humidity;
    }

    /**
     * Getter for the air co2 level
     * @return an integer value representing the air co2 level
     */
    public int getAir_co2() {
        return air_co2;
    }

    /**
     * Setter for the air co2 value
     * @param air_co2 - the new value for the air co2
     */
    public void setAir_co2(int air_co2) {
        this.air_co2 = air_co2;
    }

    /**
     * Getter for the light level
     * @return a float value representing the light level
     */
    public float getLight_level() {
        return light_level;
    }

    /**
     * Setter for the light value
     * @param light_level - the new value for the light level
     */
    public void setLight_level(float light_level) {
        this.light_level = light_level;
    }

    /**
     * An overriden version of the toString method
     *
     * @return a string representation of the sensor entry
     */
    @Override
    public String toString() {
        return "SensorEntry{" +
                "entry_key=" + entry_key +
                ", user_key=" + user_key +
                ", hardware_id='" + hardware_id + '\'' +
                ", entry_time=" + entry_time +
                ", air_temperature=" + air_temperature +
                ", air_humidity=" + air_humidity +
                ", air_co2=" + air_co2 +
                ", light_level=" + light_level +
                ", desired_air_temperature=" + desired_air_temperature +
                ", desired_air_humidity=" + desired_air_humidity +
                ", desired_air_co2=" + desired_air_co2 +
                ", desired_light_level=" + desired_light_level +
                '}';
    }
}
