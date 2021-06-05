package via.sep4.Persistence;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;
import via.sep4.Model.User;

public interface UserAPI {
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("user/name/Bob")
    Call<User> getUserBob();

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("user/name/{username}")
    Call<User> getUser(@Path("username") String username);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("user/me")
    Call<User> getMe();

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @POST("user")
    Call<User> createUser(@Body User user);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @DELETE("user/key/{user_key}")
    Call<User> delUser(@Path("user_key") int user_key);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @PATCH("user/key/{user_key}/username")
    Call<User> setUsername(@Path("user_key") int user_key,@Field("username") String username);

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @PATCH("user/key/{user_key}/username")
    Call<User> setPassword(@Path("user_key") int user_key,@Field("password") String password);
}