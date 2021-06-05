package sep4.iot.gateway.webSocket;

import org.apache.commons.codec.DecoderException;
import org.apache.commons.codec.binary.Hex;
import sep4.iot.gateway.model.SensorEntry;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * Runnable class used to manage the transmission of information between
 * the socket client and the service classes
 *
 * @author Daria Popa
 * @version 1.0
 * @since 26-05-2021
 */
public class WebSocketThread implements Runnable{

    private WebSocketClient webSocketClient;
    private int user_key;
    private ArrayList<SensorEntry> sensorEntries;
    private boolean stop;

    /**
     * Constructor used to start the web socket connection for the user
     * @param url - the user's URL created with the unique app token
     * @param user_key - the user's unique key used to differentiate threads
     */
    public WebSocketThread(String url, int user_key) {
        webSocketClient = new WebSocketClient(url);
        this.user_key=user_key;
        sensorEntries = new ArrayList<>();
        stop = false;
    }

    /**
     * Getter for the user key
     * @return an integer value representing the user_key value
     */
    public synchronized int getUser_key() {
        return user_key;
    }

    /**
     * Method for returning all the saved sensor data
     * @return an ArrayList of SensorEntry objects
     */
    public synchronized ArrayList<SensorEntry> getSensorEntries() {
        return sensorEntries;
    }

    /**
     * Method for emptying the list of sensor entries
     */
    public synchronized void clearSensorEntries() {
        sensorEntries.clear();
    }

    /**
     * Method for constructing and sending a jsonTelegram to the hardware
     * @param sensorEntry - the object containing the needed information to construct the downlink message
     */
    public synchronized void sendSensorData(SensorEntry sensorEntry){

        String data = "";
        short temp, hum,co2,light;
        temp = (short)sensorEntry.getDesired_air_temperature();
        hum = (short)sensorEntry.getDesired_air_humidity();
        co2 = (short)sensorEntry.getDesired_air_co2();
        light = (short) sensorEntry.getDesired_light_level();

        ByteBuffer bb = ByteBuffer.allocate(2);
        bb.clear();
        byte[] bytes = bb.putShort(temp).array();
        data+= Hex.encodeHexString(bytes);
        bb.clear();
        bytes = bb.putShort(hum).array();
        data+= Hex.encodeHexString(bytes);
        bb.clear();
        bytes = bb.putShort(co2).array();
        data+= Hex.encodeHexString(bytes);
        bb.clear();
        bytes=bb.putShort(light).array();
        data+= Hex.encodeHexString(bytes);
        bb.clear();

        String jsonTelegram = "{ \"cmd\" : \"tx\", \"EUI\" : \""+sensorEntry.getHardware_id()+"\", " +
                "\"port\" : 2, \"confirmed\": false, \"data\" : \""+data+"\" }";
        System.out.println("SENDING: "+jsonTelegram);
        webSocketClient.sendDownLink(jsonTelegram);
    }

    /**
     * Method for setting the stop flag of the thread to true
     */
    public synchronized void stop() {
        stop = true;
    }

    /**
     * Thread's run method, running indefinitely, until the stop flag is set to true;
     * The method periodically asks the socket for new information and if any, saves
     * it to the list of SensorEntries
     */
    @Override
    public void run() {

        while (!stop){
            System.out.println("THREAD IS RUNNING FOR USER "+user_key);
            try {
                Thread.sleep(60000);
            } catch (InterruptedException e) {
                e.printStackTrace();
                break;
            }

            String upLinkMessage = webSocketClient.getUpLink();
            if(upLinkMessage!=null){
                System.out.println("SENSOR ENTRY MESSAGE FROM THREAD: "+upLinkMessage);
                /*
                eg:{
                    "rssi": -116,
                    "seqno": 39,
                    "data": "000d0018026bda02",
                    "toa": 0,
                    "freq": 867300000,
                    "ack": false,
                    "fcnt": 0,
                    "dr": "SF12 BW125 4\/5",
                    "offline": false,
                    "bat": 255,
                    "port": 1,
                    "snr": -13,
                    "EUI": "0004A30B002528D3",
                    "cmd": "rx",
                    "ts": 1619347683413
                }
                */

                String[] lines = upLinkMessage.split(",");
                String dataLine="", tsLine="", euiLine="", cmd="";
                for (String str:lines) {
                    if(str.contains("data")){
                        dataLine=str;
                    }else if(str.contains("ts")){
                        tsLine=str;
                    }else if(str.contains("EUI")){
                        euiLine=str;
                    }else if(str.contains("cmd")){
                        cmd=str;
                    }
                }
                System.out.println("data line: "+dataLine);
                System.out.println("ts: "+tsLine);
                System.out.println("EUI: "+euiLine);

                if(cmd.contains("rx")){
                    SensorEntry sensorEntry = new SensorEntry();
                    sensorEntry.setUser_key(user_key);

                    euiLine = euiLine.split("\"")[3];
                    sensorEntry.setHardware_id(euiLine);

                    tsLine = tsLine.split(":")[1].trim();
                    tsLine = tsLine.split(" ")[0].trim();
                    sensorEntry.setEntry_time(Long.parseLong(tsLine));

                    dataLine = dataLine.split("\"")[3];
                    byte[] data= new byte[8];
                    try {
                        data = Hex.decodeHex(dataLine.toCharArray());
                    } catch (DecoderException e) {
                        e.printStackTrace();
                    }

                    int hum = ((data[0] & 0xff) << 8) | (data[1] & 0xff);
                    int temp = ((data[2] & 0xff) << 8) | (data[3] & 0xff);
                    int co2 = ((data[4] & 0xff) << 8) | (data[5] & 0xff);
                    int light = ((data[6] & 0xff) << 8) | (data[7] & 0xff);

                    sensorEntry.setAir_humidity((float) hum/10);
                    sensorEntry.setAir_temperature((float) temp / 10);
                    sensorEntry.setAir_co2(co2);
                    sensorEntry.setLight_level(light*10);

                    System.out.println("RECEIVED SENSOR ENTRY: " + sensorEntry.toString());
                    sensorEntries.add(sensorEntry);
                }
            }
        }
    }

}


