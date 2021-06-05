package sep4.iot.gateway.persistence;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.boot.system.ApplicationHome;
import sep4.iot.gateway.model.HardwareUser;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;

/**
 * Persistence class used to store and update the list of user threads
 * communicating to the Loriot server
 *
 * @author Daria Popa
 * @version 1.0
 * @since 26-05-2021
 */
public class UserThreadFile {

    private ArrayList<HardwareUser> threads;
    private final String filename;
    private File file;
    private ObjectMapper mapper;

    /**
     * Constructor used to initialise the hardware user threads list
     */
    public UserThreadFile(){
        filename = new ApplicationHome(this.getClass()).getDir() + "/users.txt";
        threads = new ArrayList<>();
        threads = readThreadsList();
    }

    /**
     * Method for adding a new hardware user to the list
     * @param user - the hardware user to be added to the list
     */
    public void addThread(HardwareUser user){
        threads.add(user);
        updateThreadList();
    }

    /**
     * Method for removing a user's information from the list
     * @param user - the user to be removed
     */
    public void removeThread(HardwareUser user){
        threads.remove(user);
        updateThreadList();
    }

    /**
     * Method for returning all the users with running threads
     * @return an ArrayList of all the HardwareUser objects in the file
     */
    public ArrayList<HardwareUser> getAllThreads(){
        threads=readThreadsList();
        return threads;
    }

    /**
     * Method checking if a user has a related thread
     * @param user_key - the unique key of the user to check for
     * @return a boolean value representing the result of the search
     */
    public boolean isUserThreadStarted(int user_key){
        for (HardwareUser user:threads) {
            if(user.getUser_key()==user_key)
                return true;
        }
        return false;
    }

    /**
     * Method for updating the local persistence with the values is the thread list
     */
    private void updateThreadList(){
        FileWriter out = null;
        mapper = new ObjectMapper();
        try {
            file = new File(filename);
            out = new FileWriter(file);
            for (HardwareUser user:threads) {
                String string = mapper.writeValueAsString(user);
                out.append(string+"\n");
                out.flush();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }finally {
            try {
                out.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

    }

    /**
     * Method for reading the values from the local persistence
     * @return an ArrayList of all the HardwareUser objects in the file
     */
    private ArrayList<HardwareUser> readThreadsList(){
        ArrayList<HardwareUser> list = new ArrayList<>();
        Scanner in = null;
        try {
            file = new File(filename);
            mapper = new ObjectMapper();
            in = new Scanner(file);
                while (in.hasNext()) {
                    String jsonLine = in.nextLine();
                    HardwareUser user = mapper.readValue(jsonLine, HardwareUser.class);
                    list.add(user);
                }
        } catch (Exception e) {
            e.printStackTrace();
        }finally {
            in.close();
        }
        return list;
    }
}
