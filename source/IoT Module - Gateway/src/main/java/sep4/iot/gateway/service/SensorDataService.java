package sep4.iot.gateway.service;

import org.springframework.stereotype.Service;
import sep4.iot.gateway.model.HardwareUser;
import sep4.iot.gateway.model.SensorEntry;
import sep4.iot.gateway.persistence.UserThreadFile;
import sep4.iot.gateway.webSocket.WebSocketThread;

import java.util.ArrayList;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.ScheduledThreadPoolExecutor;

/**
 * Implementation of the Sensor service interface
 *
 * @author Daria Popa
 * @author Natali Munk-Jakobsen
 * @version 1.0
 * @since 26-05-2021
 */
@Service
public class SensorDataService implements ISensorDataService{

    public static ExecutorService executorService ;
    private final UserThreadFile persistence;
    private static ArrayList<WebSocketThread> threads = new ArrayList<>();

    /**
     * Constructor used to initialise the persistence and listening threads
     */
    public SensorDataService() {
        if(executorService==null)
            executorService=new ScheduledThreadPoolExecutor(10); //base size for now
        persistence = new UserThreadFile();
        initialiseThreads();
    }

    /**
     * Method used to start the WebSocketThreads for the users saved in the local persistence
     */
    private void initialiseThreads(){
        try {
            for (HardwareUser user: persistence.getAllThreads()) {
                String url = "wss://iotnet.teracom.dk/app?token="+user.getUser_token()+"=";
                WebSocketThread webSocketThread = new WebSocketThread(url, user.getUser_key());
                threads.add(webSocketThread);
                executorService.submit(webSocketThread);
            }
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }

    /**
     * Getter for the sensor data from a user's threads
     * @param userKey - the user for which the data is requested
     * @return an ArrayList with the SensorEntry elements
     */
    @Override
    public ArrayList<SensorEntry> getSensorEntry(int userKey) {
        ArrayList<SensorEntry> info = new ArrayList<>();
        for (WebSocketThread hd: threads) {
            if(hd.getUser_key()==userKey){
                info.addAll(hd.getSensorEntries());
                hd.clearSensorEntries();
                return info;
            }
        }
        return info;
    }

    /**
     * Method for sending data to the hardware
     * @param sensorEntry - object containing the information needed to construct the downlink
     */
    @Override
    public void sendDataToSensor(SensorEntry sensorEntry) {
        for (WebSocketThread hd: threads) {
            if(hd.getUser_key()==sensorEntry.getUser_key()){
                hd.sendSensorData(sensorEntry);
                break;
            }
        }
    }

    /**
     * Method for creating and starting a new user's listening thread
     * @param user - the unique key and token of the new user
     */
    @Override
    public void createNewUserThread(HardwareUser user) {
        if(!persistence.isUserThreadStarted(user.getUser_key())) {
            String url = "wss://iotnet.teracom.dk/app?token="+user.getUser_token()+"=";
            persistence.addThread(user);
            WebSocketThread webSocketThread = new WebSocketThread(url, user.getUser_key());
            threads.add(webSocketThread);
            executorService.submit(webSocketThread);
        }else{
            System.out.println("THREAD ALREADY STARTED FOR USER "+user.getUser_key()+"!!!");
        }
    }

    /**
     * Method used to remove a user's thread
     * @param user_key - the unique key of the user
     */
    @Override
    public void destroyUserThread(int user_key) {
        persistence.removeThread(new HardwareUser(user_key, ""));
        for (WebSocketThread thread : threads)
            if (thread.getUser_key() == user_key)
            {
                thread.stop();
                threads.remove(thread);
                break;
            }

    }

}
