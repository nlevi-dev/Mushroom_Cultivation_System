package via.sep4.Model;

public class User {

    /**
     * @author Kristóf Lénárd
     * @version 1.0
     * This class is responsible for storing Users. It does not store passwords to ensure higher secrecy.
     * NOTE: does not use Room. All local storage uses AppData and by extenstion, SharedPreferences.
     */
    private int user_key;
    private String username;
    private String password;
    private String user_type;

    public User() {
    }

    public int getUser_key() {
        return user_key;
    }

    public void setUser_key(int user_key) {
        this.user_key = user_key;
    }

    public String getPassword() {
        return password;
    }

    public String getUsername() {
        return username;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getUser_type() {
        return user_type;
    }

    public void setUser_type(String user_type) {
        this.user_type = user_type;
    }
}
