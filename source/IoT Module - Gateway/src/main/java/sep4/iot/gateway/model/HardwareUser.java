package sep4.iot.gateway.model;

import java.io.Serializable;

/**
 * The HardwareUser object contains all the important user information for handling
 * threads listening and sending information to the hardware
 *
 * @see java.io.Serializable
 * @author Daria Popa
 * @version 1.0
 * @since 26-05-2021
 */
public class HardwareUser implements Serializable {
    /**
     * @param user_key - unique user identifier
     */
    private int user_key;
    /**
     * @param user_token - unique Loriot app token
     */
    private String user_token;

    /**
     * Default constructor
     */
    public HardwareUser(){}

    /**
     * Main constructor for HardwareUser
     * @param user_key - unique identification number for a user
     * @param user_token - unique application token for an user used to connect to the Loriot gateway
     */
    public HardwareUser(int user_key, String user_token) {
        this.user_key = user_key;
        this.user_token = user_token;
    }

    /**
     * Getter for user_key
     * @return an integer value representing the unique identification number of the user
     */
    public int getUser_key() {
        return user_key;
    }

    /**
     * Setter for user_key
     * @param user_key - the new value for the user_key
     */
    public void setUser_key(int user_key) {
        this.user_key = user_key;
    }

    /**
     * Getter for the user_token
     * @return a reference to the String value of the user's Loriot application token
     */
    public String getUser_token() {
        return user_token;
    }

    /**
     * Setter for the user_token
     * @param user_token - the new value for the Loriot application token of the user
     */
    public void setUser_token(String user_token) {
        this.user_token = user_token;
    }

    /**
     * Equals method for the HardwareUser object
     * @param object - the object to compare with
     * @return a boolean value representing the result of the comparison
     */
    @Override
    public boolean equals(Object object) {
        if (!(object instanceof HardwareUser))
            return false;
        return ((HardwareUser) object).user_key == user_key;
    }
}
