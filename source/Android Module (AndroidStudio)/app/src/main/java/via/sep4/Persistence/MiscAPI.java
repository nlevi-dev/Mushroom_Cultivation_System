package via.sep4.Persistence;

import java.util.List;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Headers;

public interface MiscAPI
{
    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("defined/mushroom/stages")
    Call<List<String>> getStages();

    @Headers({ "Content-Type: application/json;charset=UTF-8"})
    @GET("defined/mushroom/types")
    Call<List<String>> getTypes();

    //@Headers({ "Content-Type: text/plain;charset=UTF-8"})
    @GET("token")
    Call<String> getToken(@Header("Authorization") String auth);
}