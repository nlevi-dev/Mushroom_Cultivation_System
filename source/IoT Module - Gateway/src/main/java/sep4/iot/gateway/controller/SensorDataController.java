package sep4.iot.gateway.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import sep4.iot.gateway.model.HardwareUser;
import sep4.iot.gateway.model.SensorEntry;
import sep4.iot.gateway.service.SensorDataService;

import java.util.ArrayList;

/**
 * The Sensor Controller handles the requests sent by the client (Data server)
 *
 * @author Daria Popa
 * @author Mihai Anghelus
 * @version 1.0
 * @since 26-05-2021
 */
@RestController
//@RequestMapping("/SensorData")
public class SensorDataController {

    /**
     * @param service - The service is instantiated in the controller using dependency injection
     */
    @Autowired
    SensorDataService service;

    /**
     * HTTP GET controller method
     * @param user_key - the user for which data is requested
     * @return an <b>ArrayList of SensorEntry</b> elements for the specified user,
     * or an empty list on exception caught
     */
    //CRUD-Retrieve
    @GetMapping("/hardware/{user_key}")
    public ArrayList<SensorEntry> getSensorEntry(@PathVariable("user_key") final String user_key){
        try {
            int id = Integer.parseInt(user_key);
            ArrayList<SensorEntry> entry = service.getSensorEntry(id);
            return entry;
        }catch (Exception e){
            System.out.println(e.getMessage());
            return new ArrayList<>();
        }
    }

    /**
     * HTTP POST controller method for sending a downlink to the hardware
     * @param sensorEntry - object containing the necessary information to construct
     *                    a downlink message (i.e: desired value and HEWUI)
     */
    //CRUD-Create
    @PostMapping("/hardware")
    public void sendDataToSensor(@RequestBody final SensorEntry sensorEntry){
        try {
            service.sendDataToSensor(sensorEntry);
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }

    /**
     * HTTP POST controller method for starting a listening thread for a new user
     * @param user - the unique user key and loriot token used to start and identify threads
     */
    //CRUD-Create
    @PostMapping("/user")
    public void createNewUserThread(@RequestBody final HardwareUser user){
        try {
            service.createNewUserThread(user);
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }

    /**
     * HTTP DELETE controller method for deleting a listening thread
     * @param user_key - the unique identification number of the user and thread
     */
    //CRUD-Delete
    @DeleteMapping("/user/{user_key}")
    public void destroyUserThread(@PathVariable("user_key") final String user_key){
        try {
            int id = Integer.parseInt(user_key);
            service.destroyUserThread(id);
        }catch (Exception e){
            System.out.println(e.getMessage());
        }
    }

}
