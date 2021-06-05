package sep4.iot.gateway.service;

import sep4.iot.gateway.model.HardwareUser;
import sep4.iot.gateway.model.SensorEntry;

import java.util.ArrayList;

/**
 * Sensor service interface
 *
 * @author Mihai Anghelus
 * @author Daria Popa
 * @version 1.0
 * @since 26-05-2021
 */
public interface ISensorDataService {

    ArrayList<SensorEntry> getSensorEntry(int userKey);
    void sendDataToSensor(SensorEntry sensorEntry);
    void createNewUserThread(HardwareUser user);
    void destroyUserThread(int user_key);
}
